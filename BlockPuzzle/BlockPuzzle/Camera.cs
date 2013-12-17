using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BlockPuzzle
{
    public class Camera
    {

        public Matrix rotation = Matrix.Identity;
        public Vector3 position = Vector3.Zero;

        // Simply feed this camera the position of whatever you want its target to be
        public Vector3 targetPosition = Vector3.Zero;

        public Matrix View = Matrix.Identity;
        public Matrix Projection = Matrix.Identity;
        private float zoom = 180.0f;
        public float Zoom
        {
            get
            {
                return zoom;
            }
            set
            {    // Keep zoom within range
                zoom = MathHelper.Clamp(value, zoomMin, zoomMax);
            }

        }


        private float horizontalAngle = .6374611f;//MathHelper.PiOver2;
        public float HorizontalAngle
        {
            get
            {
                return horizontalAngle;
            }
            set
            {    // Keep horizontalAngle between -pi and pi.
                horizontalAngle = MathHelper.WrapAngle(value);
            }
        }

        private float verticalAngle = 1.154129f;//MathHelper.PiOver2;
        public float VerticalAngle
        {
            get
            {
                return verticalAngle;
            }
            set
            {    // Keep vertical angle within tolerances
                verticalAngle = MathHelper.WrapAngle(value);
            }
        }

        private const float verticalAngleMin = 0.01f;
        private const float verticalAngleMax = MathHelper.Pi - 0.01f;
        private const float zoomMin = 0.1f;
        private const float zoomMax = 1000.0f;


        // FOV is in radians
        // screenWidth and screenHeight are pixel values. They're floats because we need to divide them to get an aspect ratio.
        public Camera(float FOV, float aspectRatio, float nearPlane, float farPlane)
        {
            
            if (nearPlane < 0.1f)
                throw new Exception("nearPlane must be greater than 0.1");

            this.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(FOV), aspectRatio,
                                                                        nearPlane, farPlane);
        }

        public void Update(GameTime gameTime)
        {
            // Start with an initial offset
            Vector3 cameraPosition = new Vector3(0.0f, zoom, 0.0f);

            // Rotate vertically
            cameraPosition = Vector3.Transform(cameraPosition, Matrix.CreateFromYawPitchRoll(horizontalAngle, verticalAngle, 0));

            position = cameraPosition + targetPosition;
//            this.LookAt(targetPosition);

            // Compute view matrix
            this.View = Matrix.CreateLookAt(this.position,
                                                  targetPosition,
                                                  Vector3.Up);
        }

//        /// <summary>
//        /// Points camera in direction of any position.
//        /// </summary>
//        /// <param name="targetPos">Target position for camera to face.</param>
//        public void LookAt(Vector3 targetPos)
//        {
//            Vector3 newForward = targetPos - this.position;
//            newForward.Normalize();
//            this.rotation.Forward = newForward;
//
//            Vector3 referenceVector = Vector3.UnitY;
//
//            // On the slim chance that the camera is pointer perfectly parallel with the Y Axis, we cannot
//            // use cross product with a parallel axis, so we change the reference vector to the forward axis (Z).
//            if (this.rotation.Forward.Y == referenceVector.Y || this.rotation.Forward.Y == -referenceVector.Y)
//            {
//                referenceVector = Vector3.UnitZ;
//            }
//
//            this.rotation.Right = Vector3.Cross(this.rotation.Forward, referenceVector);
//            this.rotation.Up = Vector3.Cross(this.rotation.Right, this.rotation.Forward);
//        }

    }
}
