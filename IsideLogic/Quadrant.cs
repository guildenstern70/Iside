using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IsideLogic
{
    public enum Quadrant
    {
        UPPER_LEFT,
        LOWER_LEFT,
        UPPER_RIGHT,
        LOWER_RIGHT
    }

    public class HashQuadrant
    {
        private Quadrant _quadrant;
        private bool _isLeft;

        public HashQuadrant(Quadrant q)
        {
            this._quadrant = q;

            switch (this._quadrant)
            {
                case Quadrant.LOWER_LEFT:
                    this._isLeft = true;
                    this._isUpper = false;
                    break;

                case Quadrant.LOWER_RIGHT:
                    this._isLeft = false;
                    this._isUpper = false;
                    break;

                case Quadrant.UPPER_LEFT:
                    this._isLeft = true;
                    this._isUpper = true;
                    break;

                case Quadrant.UPPER_RIGHT:
                    this._isLeft = false;
                    this._isUpper = true;
                    break;
            }
        }

        public bool IsLeft
        {
            get { return _isLeft; }
        }
        private bool _isUpper;

        public bool IsUpper
        {
            get { return _isUpper; }
        }
    }
}
