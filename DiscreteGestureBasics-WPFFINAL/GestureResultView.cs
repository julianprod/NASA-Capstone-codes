//------------------------------------------------------------------------------
// <copyright file="GestureResultView.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.Samples.Kinect.DiscreteGestureBasics
{
    using System;
    using System.Threading;
    using System.IO.Ports;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    /// <summary>
    /// Stores discrete gesture results for the GestureDetector.
    /// Properties are stored/updated for display in the UI.
    /// </summary>
    public sealed class GestureResultView : INotifyPropertyChanged
    {
        SerialPort sp = new SerialPort();

        /// <summary> Image to show when the 'detected' property is true for a tracked body </summary>
        private readonly ImageSource seatedImage = new BitmapImage(new Uri(@"Images\Seated.png", UriKind.Relative));

        /// <summary> Image to show when the 'detected' property is false for a tracked body </summary>
        private readonly ImageSource notSeatedImage = new BitmapImage(new Uri(@"Images\NotSeated.png", UriKind.Relative));

        /// <summary> Image to show when the body associated with the GestureResultView object is not being tracked </summary>
        private readonly ImageSource notTrackedImage = new BitmapImage(new Uri(@"Images\NotTracked.png", UriKind.Relative));

        /// <summary> Array of brush colors to use for a tracked body; array position corresponds to the body colors used in the KinectBodyView class </summary>
        private readonly Brush[] trackedColors = new Brush[] { Brushes.Red, Brushes.Orange, Brushes.Green, Brushes.Blue, Brushes.Indigo, Brushes.Violet };

        /// <summary> Brush color to use as background in the UI </summary>
        private Brush bodyColor = Brushes.YellowGreen;

        /// <summary> The body index (0-5) associated with the current gesture detector </summary>
        private int bodyIndex = 0;

        private int x = 0;

        

        /// <summary> Current confidence value reported by the discrete gesture </summary>
        private float confidence1 = 0.0f;

        

        /// <summary> True, if the discrete gesture is currently being detected </summary>
        private bool detected1 = false;

        private bool detected2 = false;

        private bool detected3 = false;

        private bool detected4 = false;

        private bool detected5 = false;

        private bool detected6 = false;

        private bool detected7 = false;

        private bool detected8 = false;

        /// <summary> Image to display in UI which corresponds to tracking/detection state </summary>
        private ImageSource imageSource = null;
        
        /// <summary> True, if the body is currently being tracked </summary>
        private bool isTracked = false;

        /// <summary>
        /// Initializes a new instance of the GestureResultView class and sets initial property values
        /// </summary>
        /// <param name="bodyIndex">Body Index associated with the current gesture detector</param>
        /// <param name="isTracked">True, if the body is currently tracked</param>
        /// <param name="detected">True, if the gesture is currently detected for the associated body</param>
        /// <param name="confidence">Confidence value for detection of the 'Seated' gesture</param>
        public GestureResultView(int x, int bodyIndex, bool isTracked, bool detected1, float confidence1, bool detected2, bool detected3, bool detected4, bool detected5, bool detected6, bool detected7, bool detected8)
        {
            this.x = x;
            this.BodyIndex = bodyIndex;
            this.IsTracked = isTracked;
            this.Detected1 = detected1;
            this.Detected2 = detected2;
            this.Detected3 = detected3;
            this.Detected4 = detected4;
            this.Detected5 = detected5;
            this.Detected6 = detected6;
            this.Detected7 = detected7;
            this.detected8 = detected8;
            this.Confidence1 = confidence1;
            this.ImageSource = this.notTrackedImage;
        }

        /// <summary>
        /// INotifyPropertyChangedPropertyChanged event to allow window controls to bind to changeable data
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary> 
        /// Gets the body index associated with the current gesture detector result 
        /// </summary>
        public int BodyIndex
        {
            get
            {
                return this.bodyIndex;
            }

            private set
            {
                if (this.bodyIndex != value)
                {
                    this.bodyIndex = value;
                    this.NotifyPropertyChanged();
                }
            }
        }

        /// <summary> 
        /// Gets the body color corresponding to the body index for the result
        /// </summary>
        public Brush BodyColor
        {
            get
            {
                return this.bodyColor;
            }

            private set
            {
                if (this.bodyColor != value)
                {
                    this.bodyColor = value;
                    this.NotifyPropertyChanged();
                }
            }
        }

        /// <summary> 
        /// Gets a value indicating whether or not the body associated with the gesture detector is currently being tracked 
        /// </summary>
        public bool IsTracked 
        {
            get
            {
                return this.isTracked;
            }

            private set
            {
                if (this.IsTracked != value)
                {
                    this.isTracked = value;
                    this.NotifyPropertyChanged();
                }
            }
        }

        /// <summary> 
        /// Gets a value indicating whether or not the discrete gesture has been detected
        /// </summary>
        public bool Detected1 
        {
            get
            {
                return this.detected1;
            }

            private set
            {
                if (this.detected1 != value)
                {
                    this.detected1 = value;
                    this.NotifyPropertyChanged();
                }
            }
        }

        public bool Detected2
        {
            get
            {
                return this.detected2;
            }

            private set
            {
                if (this.detected2 != value)
                {
                    this.detected2 = value;
                    this.NotifyPropertyChanged();
                }
            }
        }

        public bool Detected3
        {
            get
            {
                return this.detected3;
            }

            private set
            {
                if (this.detected3 != value)
                {
                    this.detected3 = value;
                    this.NotifyPropertyChanged();
                }
            }
        }

        public bool Detected4
        {
            get
            {
                return this.detected4;
            }

            private set
            {
                if (this.detected4 != value)
                {
                    this.detected4 = value;
                    this.NotifyPropertyChanged();
                }
            }
        }

        public bool Detected5
        {
            get
            {
                return this.detected5;
            }

            private set
            {
                if (this.detected5 != value)
                {
                    this.detected5 = value;
                    this.NotifyPropertyChanged();
                }
            }
        }

        public bool Detected6
        {
            get
            {
                return this.detected6;
            }

            private set
            {
                if (this.detected6 != value)
                {
                    this.detected6 = value;
                    this.NotifyPropertyChanged();
                }
            }
        }

        public bool Detected7
        {
            get
            {
                return this.detected7;
            }

            private set
            {
                if (this.detected7 != value)
                {
                    this.detected7 = value;
                    this.NotifyPropertyChanged();
                }
            }
        }

        public bool Detected8
        {

            get
            {
                return this.detected8;
            }

            private set
            {
                if(this.detected8 != value)
                {
                    this.detected8 = value;
                    this.NotifyPropertyChanged();
                }
            }
        }

        /// <summary> 
        /// Gets a float value which indicates the detector's confidence that the gesture is occurring for the associated body 
        /// </summary>
        public float Confidence1
        {
            get
            {
                return this.confidence1;
            }

            private set
            {
                if (this.confidence1 != value)
                {
                    this.confidence1 = value;
                    this.NotifyPropertyChanged();
                }
            }
        }

       

        /// <summary> 
        /// Gets an image for display in the UI which represents the current gesture result for the associated body 
        /// </summary>
        public ImageSource ImageSource
        {
            get
            {
                return this.imageSource;
            }

            private set
            {
                if (this.ImageSource != value)
                {
                    this.imageSource = value;
                    this.NotifyPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Updates the values associated with the discrete gesture detection result
        /// </summary>
        /// <param name="isBodyTrackingIdValid">True, if the body associated with the GestureResultView object is still being tracked</param>
        /// <param name="isGestureDetected">True, if the discrete gesture is currently detected for the associated body</param>
        /// <param name="detectionConfidence">Confidence value for detection of the discrete gesture</param>
        public void UpdateGestureResult(int x, bool isBodyTrackingIdValid, bool isGestureDetected1, float detectionConfidence1, bool isGestureDetected2, bool isGestureDetected3, bool isGestureDetected4, bool isGestureDetected5, bool isGestureDetected6, bool isGestureDetected7, bool isGestureDetected8)
        {
                sp.PortName = "COM8";
                sp.BaudRate = 9600;
                sp.Open();

                sp.Write("c");
            

            this.IsTracked = isBodyTrackingIdValid;


            this.Confidence1 = 0.0f;

            this.Detected1 = isGestureDetected1;
            this.Detected2 = isGestureDetected2;
            this.Detected3 = isGestureDetected3;
            this.Detected4 = isGestureDetected4;
            this.Detected5 = isGestureDetected5;
            this.Detected6 = isGestureDetected6;
            this.Detected7 = isGestureDetected7;
            this.Detected8 = isGestureDetected8;

            if (!this.IsTracked)
            {
                this.ImageSource = this.notTrackedImage;
                this.Detected1 = false;
                this.Detected2 = false;
                this.Detected3 = false;
                this.Detected4 = false;
                this.Detected5 = false;
                this.Detected6 = false;
                this.Detected7 = false;
                this.Detected8 = false;
                this.BodyColor = Brushes.Gray;
            }
            else
            {
           

                this.BodyColor = this.trackedColors[this.BodyIndex];

                if (this.Detected1)
                {

                    this.Confidence1 = detectionConfidence1;
                    // this.ImageSource = this.seatedImage;
                    sp.Write("A");
                    this.ImageSource = this.seatedImage;
                }
                else
                {
                 //   sp.Write("a");
                }
                 
                
                //this.ImageSource = this.notSeatedImage;
                if (this.Detected2)
                {
                    this.Confidence1 = detectionConfidence1;
                    //this.ImageSource = this.seatedImage;
                    sp.Write("E");
                }
                

                 if (this.Detected3)
                 {
                     this.Confidence1 = detectionConfidence1;
                     
                     sp.Write("F");
                 }

                 if(this.Detected4)
                 {
                     this.Confidence1 = detectionConfidence1;

                     sp.Write("B");
                 }

                if(this.Detected5)
                {
                    this.Confidence1 = detectionConfidence1;

                    sp.Write("D");
                }

                if(this.Detected6)
                {
                    this.Confidence1 = detectionConfidence1;

                    sp.Write("C");
                }

                if(this.Detected7)
                {
                    this.Confidence1 = detectionConfidence1;

                    sp.Write("I");
                }

                if(this.Detected8)
                {
                    this.Confidence1 = detectionConfidence1;

                    sp.Write("H");
                }

                 if (!this.Detected1 && !this.Detected2 && !this.Detected3 && !this.Detected4 && !this.Detected5 && !this.Detected6 && !this.Detected7 && !this.Detected8)
                {
                    this.ImageSource = this.notTrackedImage;
                }
                else
                {
                    this.ImageSource = this.notSeatedImage;
                }

            }

            
            sp.Close();
            Thread.Sleep(20);
            sp.Dispose();
               
        }

        /// <summary>
        /// Notifies UI that a property has changed
        /// </summary>
        /// <param name="propertyName">Name of property that has changed</param> 
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
            
        }
    }
}
