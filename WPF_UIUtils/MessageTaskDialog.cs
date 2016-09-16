using System;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace WPFUtils
{
    public class Wpf32Window : System.Windows.Forms.IWin32Window
    {
        public IntPtr Handle { get; private set; }

        public Wpf32Window(Window wpfWindow)
        {
            Handle = new WindowInteropHelper(wpfWindow).Handle;
        }
    }

    public enum TaskDialogType
    {
        INFO,
        WARNING,
        QUESTION,
        ERROR
    }

    public static class MessageTaskDialog
    {

        public static DialogResult Show(Window caller, string appName, string message, string instruction, TaskDialogType type)
        {
            MessageBoxIcon icon;
            MessageBoxButtons buttons = MessageBoxButtons.OK;

            switch (type)
            {
                case TaskDialogType.INFO:
                default:
                    icon = MessageBoxIcon.Information;
                    break;

                case TaskDialogType.WARNING:
                    icon = MessageBoxIcon.Warning;
                    break;

                case TaskDialogType.QUESTION:
                    icon = MessageBoxIcon.Question;
                    buttons = MessageBoxButtons.YesNo;
                    break;

                case TaskDialogType.ERROR:
                    icon = MessageBoxIcon.Stop;
                    break;
            }

            return Show(caller, appName, message, instruction, buttons, icon);
        }


        /// <summary>
        /// Shows a message dialog. It shows a TaskDialog on Vista & 7.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="instruction">The instruction.</param>
        /// <param name="buttons">The buttons.</param>
        /// <param name="icon">The icon.</param>
        /// <returns></returns>
        public static DialogResult Show(Window caller, string appName, string message, string instruction, 
                                        MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            DialogResult dr = DialogResult.OK;

            if ((TaskDialog.IsPlatformSupported) && (icon != MessageBoxIcon.Question))
            {
                TaskDialog td = MessageTaskDialog.GetTaskDialog(appName, message, instruction, icon, buttons);
                dr = MessageTaskDialog.GetDialogResult(td.Show());
            }
            else
            {
                Wpf32Window form = new Wpf32Window(caller);
                dr = System.Windows.Forms.MessageBox.Show(form, message, appName, buttons, icon);
            }

            return dr;
        }

        private static DialogResult GetDialogResult(TaskDialogResult taskResult)
        {
            DialogResult dr;

            switch (taskResult)
            {
                case TaskDialogResult.Cancel:
                    dr = DialogResult.Cancel;
                    break;

                case TaskDialogResult.Retry:
                    dr = DialogResult.Retry;
                    break;

                case TaskDialogResult.Ok:
                default:
                    dr = DialogResult.OK;
                    break;
            }

            return dr;
        }

        private static TaskDialog GetTaskDialog(string appName, string message, string instruction, MessageBoxIcon icon, 
                                                MessageBoxButtons buttons)
        {
            TaskDialog td = new TaskDialog();
            TaskDialogStandardButtons taskButtons = TaskDialogStandardButtons.None;

            // Buttons
            if (buttons == MessageBoxButtons.OK)
            {
                taskButtons |= TaskDialogStandardButtons.Ok;
            }
            else if (buttons == MessageBoxButtons.YesNo)
            {
                taskButtons |= TaskDialogStandardButtons.Yes;
                taskButtons |= TaskDialogStandardButtons.No;
            }
            else if (buttons == MessageBoxButtons.RetryCancel)
            {
                taskButtons |= TaskDialogStandardButtons.Retry;
                taskButtons |= TaskDialogStandardButtons.Cancel;
            }
            else
            {
                taskButtons |= TaskDialogStandardButtons.Ok;
                taskButtons |= TaskDialogStandardButtons.Cancel;
            }
            td.StandardButtons = taskButtons;

            // Icon
            if ((icon == MessageBoxIcon.Error) || (icon == MessageBoxIcon.Stop))
            {
                td.Icon = TaskDialogStandardIcon.Error;
            }
            else if (icon == MessageBoxIcon.Information)
            {
                td.Icon = TaskDialogStandardIcon.Information;
            }
            else if ((icon == MessageBoxIcon.Warning) || (icon == MessageBoxIcon.Asterisk))
            {
                td.Icon = TaskDialogStandardIcon.Warning;
            }
            else
            {
                td.Icon = TaskDialogStandardIcon.Information;
            }

            // Prompts
            td.Caption = appName;
            td.InstructionText = instruction;
            td.Text = message;

            return td;

        }
    }
}
