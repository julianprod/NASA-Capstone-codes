//------------------------------------------------------------------------------
// <copyright file="GestureDetector.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.Samples.Kinect.DiscreteGestureBasics
{
    using System;
    using System.IO.Ports;
    using System.Collections.Generic;
    using Microsoft.Kinect;
    using Microsoft.Kinect.VisualGestureBuilder;

    /// <summary>
    /// Gesture Detector class which listens for VisualGestureBuilderFrame events from the service
    /// and updates the associated GestureResultView object with the latest results for the 'Seated' gesture
    /// </summary>
    public class GestureDetector : IDisposable
    {
        /// <summary> Path to the gesture database that was trained with VGB </summary>
        private readonly string gestureDatabase = @"Database\handactions3.gbd";
        

        /// <summary> Name of the discrete gesture in the database that we want to track </summary>
        private readonly string gofor = "go_forward";
        private readonly string backr = "backright";
        private readonly string backl = "backleft";
        private readonly string back = "go_back";
        private readonly string goleft = "go_left";
        private readonly string goright = "go_right";
        private readonly string left360 = "left360_Left";
        private readonly string right360 = "right360";



        /// <summary> Gesture frame source which should be tied to a body tracking ID </summary>
        private VisualGestureBuilderFrameSource vgbFrameSource = null;

        /// <summary> Gesture frame reader which will handle gesture events coming from the sensor </summary>
        private VisualGestureBuilderFrameReader vgbFrameReader = null;

         
        /// <summary>
        /// Initializes a new instance of the GestureDetector class along with the gesture frame source and reader
        /// </summary>
        /// <param name="kinectSensor">Active sensor to initialize the VisualGestureBuilderFrameSource object with</param>
        /// <param name="gestureResultView">GestureResultView object to store gesture results of a single body to</param>
        public GestureDetector(KinectSensor kinectSensor, GestureResultView gestureResultView)
        {

            
            if (kinectSensor == null)
            {
                throw new ArgumentNullException("kinectSensor");
            }

            if (gestureResultView == null)
            {
                throw new ArgumentNullException("gestureResultView");
            }
            
            this.GestureResultView = gestureResultView;
            
            // create the vgb source. The associated body tracking ID will be set when a valid body frame arrives from the sensor.
            this.vgbFrameSource = new VisualGestureBuilderFrameSource(kinectSensor, 0);
            this.vgbFrameSource.TrackingIdLost += this.Source_TrackingIdLost;

            // open the reader for the vgb frames
            this.vgbFrameReader = this.vgbFrameSource.OpenReader();
            if (this.vgbFrameReader != null)
            {
                this.vgbFrameReader.IsPaused = true;
                this.vgbFrameReader.FrameArrived += this.Reader_GestureFrameArrived;
            }

            // load the 'Seated' gesture from the gesture database
            using (VisualGestureBuilderDatabase database = new VisualGestureBuilderDatabase(this.gestureDatabase))
            {
                
              
                // we could load all available gestures in the database with a call to vgbFrameSource.AddGestures(database.AvailableGestures), 
                // but for this program, we only want to track one discrete gesture from the database, so we'll load it by name
                foreach (Gesture gesture in database.AvailableGestures)
                {

                   // this.vgbFrameSource.AddGestures(database.AvailableGestures);

                    if (gesture.Name.Equals(this.gofor))
                    {
                        this.vgbFrameSource.AddGesture(gesture);
                        
                    }

                    if (gesture.Name.Equals(this.backr))
                    {
                        this.vgbFrameSource.AddGesture(gesture);

                    }

                    if(gesture.Name.Equals(this.backl))
                    {
                        this.vgbFrameSource.AddGesture(gesture);
                    }

                    if(gesture.Name.Equals(this.back))
                    {
                        this.vgbFrameSource.AddGesture(gesture);
                    }

                    if (gesture.Name.Equals(this.goleft))
                    {
                        this.vgbFrameSource.AddGesture(gesture);
                    }

                    if (gesture.Name.Equals(this.goright))
                    {
                        this.vgbFrameSource.AddGesture(gesture);
                    }

                    if (gesture.Name.Equals(this.left360))
                    {
                        this.vgbFrameSource.AddGesture(gesture);
                    }

                    if (gesture.Name.Equals(this.right360))
                    {
                        this.vgbFrameSource.AddGesture(gesture);
                    }
                                       
                    
                }
               
            }
        }

        /// <summary> Gets the GestureResultView object which stores the detector results for display in the UI </summary>
        public GestureResultView GestureResultView { get; private set; }

        /// <summary>
        /// Gets or sets the body tracking ID associated with the current detector
        /// The tracking ID can change whenever a body comes in/out of scope
        /// </summary>
        public ulong TrackingId
        {
            get
            {
                return this.vgbFrameSource.TrackingId;
            }

            set
            {
                if (this.vgbFrameSource.TrackingId != value)
                {
                    this.vgbFrameSource.TrackingId = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not the detector is currently paused
        /// If the body tracking ID associated with the detector is not valid, then the detector should be paused
        /// </summary>
        public bool IsPaused
        {
            get
            {
                return this.vgbFrameReader.IsPaused;
            }

            set
            {
                if (this.vgbFrameReader.IsPaused != value)
                {
                    this.vgbFrameReader.IsPaused = value;
                }
            }
        }

        /// <summary>
        /// Disposes all unmanaged resources for the class
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the VisualGestureBuilderFrameSource and VisualGestureBuilderFrameReader objects
        /// </summary>
        /// <param name="disposing">True if Dispose was called directly, false if the GC handles the disposing</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.vgbFrameReader != null)
                {
                    this.vgbFrameReader.FrameArrived -= this.Reader_GestureFrameArrived;
                    this.vgbFrameReader.Dispose();
                    this.vgbFrameReader = null;
                }

                if (this.vgbFrameSource != null)
                {
                    this.vgbFrameSource.TrackingIdLost -= this.Source_TrackingIdLost;
                    this.vgbFrameSource.Dispose();
                    this.vgbFrameSource = null;
                }
            }
        }

        /// <summary>
        /// Handles gesture detection results arriving from the sensor for the associated body tracking Id
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void Reader_GestureFrameArrived(object sender, VisualGestureBuilderFrameArrivedEventArgs e)
        {
           
            VisualGestureBuilderFrameReference frameReference = e.FrameReference;
            using (VisualGestureBuilderFrame frame = frameReference.AcquireFrame())
            {
                if (frame != null)
                {
                    // get the discrete gesture results which arrived with the latest frame
                    IReadOnlyDictionary<Gesture, DiscreteGestureResult> discreteResults = frame.DiscreteGestureResults;

                    if (discreteResults != null)
                    {
                        // we only have one gesture in this source object, but you can get multiple gestures
                        foreach (Gesture gesture in this.vgbFrameSource.Gestures)
                        {


                            if (gesture.Name.Equals(this.gofor) && gesture.GestureType == GestureType.Discrete)
                            {
                                DiscreteGestureResult result1 = null;
                                discreteResults.TryGetValue(gesture, out result1);

                                if (result1 != null)
                                {
                                    // update the GestureResultView object with new gesture result values
                                    this.GestureResultView.UpdateGestureResult(0, true, result1.Detected, result1.Confidence, false, false, false, false, false, false, false);

                                }
                            }
                                
                             if (gesture.Name.Equals(this.backr) && gesture.GestureType == GestureType.Discrete)
                                    {
                                      
                                        DiscreteGestureResult result2 = null;
                                        discreteResults.TryGetValue(gesture, out result2);

                                        if (result2 != null)
                                        {
                                            // update the GestureResultView object with new gesture result values
                                            this.GestureResultView.UpdateGestureResult(0, true, false, result2.Confidence, false, result2.Detected, false, false, false, false, false);

                                        }
                                    }
                             
                            if(gesture.Name.Equals(this.backl) && gesture.GestureType == GestureType.Discrete)
                            {

                                DiscreteGestureResult result3 = null;
                                discreteResults.TryGetValue(gesture, out result3);

                                if(result3 != null)
                                {
                                    this.GestureResultView.UpdateGestureResult(0, true, false, result3.Confidence, false, result3.Detected, false, false, false, false, false);
                                }
                            }

                             if (gesture.Name.Equals(this.back) && gesture.GestureType == GestureType.Discrete)
                             {

                                 DiscreteGestureResult result4 = null;
                                 discreteResults.TryGetValue(gesture, out result4);

                                 if (result4 != null)
                                 {
                                     // update the GestureResultView object with new gesture result values
                                     this.GestureResultView.UpdateGestureResult(0, true, false, result4.Confidence, false, false, result4.Detected, false, false, false, false);

                                 }
                             }

                             if (gesture.Name.Equals(this.goleft) && gesture.GestureType == GestureType.Discrete)
                             {

                                 DiscreteGestureResult result5 = null;
                                 discreteResults.TryGetValue(gesture, out result5);

                                 if (result5 != null)
                                 {
                                     // update the GestureResultView object with new gesture result values
                                     this.GestureResultView.UpdateGestureResult(0, true, false, result5.Confidence, false, false, false, result5.Detected, false, false, false);

                                 }
                             }

                             if (gesture.Name.Equals(this.goright) && gesture.GestureType == GestureType.Discrete)
                             {

                                 DiscreteGestureResult result6 = null;
                                 discreteResults.TryGetValue(gesture, out result6);

                                 if (result6 != null)
                                 {
                                     // update the GestureResultView object with new gesture result values
                                     this.GestureResultView.UpdateGestureResult(0, true, false, result6.Confidence, false, false, false, false, result6.Detected, false, false);

                                 }
                             }

                             if (gesture.Name.Equals(this.left360) && gesture.GestureType == GestureType.Discrete)
                             {

                                 DiscreteGestureResult result7 = null;
                                 discreteResults.TryGetValue(gesture, out result7);

                                 if (result7 != null)
                                 {
                                     // update the GestureResultView object with new gesture result values
                                     this.GestureResultView.UpdateGestureResult(0, true, false, result7.Confidence, false, false, false, false, false, result7.Detected, false);

                                 }
                             }

                             if (gesture.Name.Equals(this.right360) && gesture.GestureType == GestureType.Discrete)
                             {

                                 DiscreteGestureResult result8 = null;
                                 discreteResults.TryGetValue(gesture, out result8);

                                 if (result8 != null)
                                 {
                                     // update the GestureResultView object with new gesture result values
                                     this.GestureResultView.UpdateGestureResult(0, true, false, result8.Confidence, false, false, false, false, false, false, result8.Detected);

                                 }
                             }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Handles the TrackingIdLost event for the VisualGestureBuilderSource object
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void Source_TrackingIdLost(object sender, TrackingIdLostEventArgs e)
        {
            // update the GestureResultView object to show the 'Not Tracked' image in the UI
            this.GestureResultView.UpdateGestureResult(0, false, false, 0.0f, false, false, false, false, false, false, false);
        }
    }
}
