using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IsideFolder.Properties;

namespace IsideFolder
{
    internal class RecentItems
    {
        /// <summary>
        /// Gets the items.
        /// </summary>
        /// <value>
        /// The items.
        /// </value>
        public List<string> Items { get; private set; }

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>
        /// The count.
        /// </value>
        public int Count 
        {
            get
            {
                return this.Items.Count;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RecentItems" /> class.
        /// </summary>
        public RecentItems()
        {
            this.Items = new List<string>();
            this.Init();
        }

        /// <summary>
        /// Refreshes this instance.
        /// </summary>
        public void Refresh()
        {
            this.Items.Clear();
            this.Init();
        }

        /// <summary>
        /// Saves this instance.
        /// </summary>
        public void Save()
        {
            if ((this.Count > 0) && (!String.IsNullOrEmpty(this.Items[0])))
            {
                if (this.Items[0] != Settings.Default.RecentComp1)
                {
                    System.Diagnostics.Debug.WriteLine("Saving 0 " + this.Items[0]);
                    Settings.Default.RecentComp1 = this.Items[0];
                }
            }
            else
            {
                Settings.Default.RecentComp1 = "";
            }

            if ((this.Count > 1) && (!String.IsNullOrEmpty(this.Items[1])))
            {
                if (this.Items[1] != Settings.Default.RecentComp2)
                {
                    System.Diagnostics.Debug.WriteLine("Saving 1 " + this.Items[1]);
                    Settings.Default.RecentComp2 = this.Items[1];
                }
            }
            else
            {
                Settings.Default.RecentComp2 = "";
            }

            if ((this.Count > 2) && (!String.IsNullOrEmpty(this.Items[2])))
            {
                if (this.Items[2] != Settings.Default.RecentComp3)
                {
                    System.Diagnostics.Debug.WriteLine("Saving 2 " + this.Items[2]);
                    Settings.Default.RecentComp3 = this.Items[2];
                }
            }
            else
            {
                Settings.Default.RecentComp3 = "";
            }

            if ((this.Count > 3) && (!String.IsNullOrEmpty(this.Items[3])))
            {
                if (this.Items[3] != Settings.Default.RecentComp4)
                {
                    System.Diagnostics.Debug.WriteLine("Saving 3 " + this.Items[3]);
                    Settings.Default.RecentComp4 = this.Items[3];
                }
            }
            else
            {
                Settings.Default.RecentComp4 = "";
            }

            Settings.Default.Save();
        }

        /// <summary>
        /// Removes the item.
        /// </summary>
        /// <param name="s">The s.</param>
        public void RemoveItem(string s)
        {
            if (this.Items.Contains(s))
            {
                this.Items.Remove(s);
            }
        }

        /// <summary>
        /// Adds the item.
        /// </summary>
        /// <param name="s">The s.</param>
        public void AddItem(string s)
        {
            // Do nothing if the same item is present
            foreach (string item in this.Items)
            {
                if (s.Equals(item))
                {
                    return;
                }
            }

            if (this.Count < 4)
            {
                if (!String.IsNullOrEmpty(s))
                {
                    this.Items.Insert(0, s);
                }
            }
            else
            {
                this.Items.RemoveAt(3); // Remove fourth
                this.AddItem(s);
            }
        }

        private void Init()
        {
            this.AddItem(Settings.Default.RecentComp1);
            this.AddItem(Settings.Default.RecentComp2);
            this.AddItem(Settings.Default.RecentComp3);
            this.AddItem(Settings.Default.RecentComp4);
        }



    }
}
