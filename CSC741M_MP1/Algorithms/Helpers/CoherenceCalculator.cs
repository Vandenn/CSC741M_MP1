using ColorMine.ColorSpaces;
using CSC741M_MP1.Algorithms.Model;
using CSC741M_MP1.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSC741M_MP1.Algorithms.Helpers
{
    /// <summary>
    /// Class for calculating an image's CCV or Color Coherence Vector.
    /// </summary>
    public class CoherenceCalculator
    {
        private int[,] image; // Discretized color 2D array based on image pixels
        private int connectedCount; // Determines what the minimum cluster count is to be considered coherent
        private List<Point> startingPointQueue; // List for holding the starting points when checking for coherent areas
        private Settings settings; // Reference to settings singleton

        private int imageHeight;
        private int imageWidth;
        private double borderPercentage;
        private double imageYBorder;
        private double imageXBorder;

        public CoherenceCalculator(Luv[,] rawImage)
        {
            settings = Settings.getSettings();
            image = convertImageToLuvIndex(rawImage);
            connectedCount = (int)Math.Floor(image.GetLength(0) * image.GetLength(1) * settings.ConnectednessThreshold);
            startingPointQueue = new List<Point>();

            imageHeight = image.GetLength(0);
            imageWidth = image.GetLength(1);
            borderPercentage = ((1 - settings.CenterAmount) / 2);
            imageYBorder = imageHeight * borderPercentage;
            imageXBorder = imageWidth * borderPercentage;
        }

        /// <summary>
        /// Function for discretizing the color space of the image.
        /// </summary>
        private int[,] convertImageToLuvIndex(Luv[,] image)
        {
            int[,] converted = new int[image.GetLength(0), image.GetLength(1)];
            for (int i = 0; i < image.GetLength(0); i++)
            {
                for (int j = 0; j < image.GetLength(1); j++)
                {
                    converted[i, j] = CIEConvert.LuvIndexOf(image[i, j]);
                }
            }

            return converted;
        }

        public Dictionary<int, CoherencePair> generateCoherenceVector()
        {
            Dictionary<int, CoherencePair> vector = new Dictionary<int, CoherencePair>();

            startingPointQueue.Add(new Point(0, 0));

            Point currentPoint;
            int currentColor;
            int currentClusterCount;
            while (startingPointQueue.Count > 0)
            {
                currentPoint = startingPointQueue[0];
                currentColor = image[currentPoint.Y, currentPoint.X];
                startingPointQueue.RemoveAt(0);
                if (currentColor >= 0)
                {
                    currentClusterCount = getClusterCount(currentPoint);
                    if (vector.ContainsKey(currentColor))
                    {
                        if (currentClusterCount >= connectedCount)
                        {
                            vector[currentColor].coherent += currentClusterCount;
                        }
                        else
                        {
                            vector[currentColor].nonCoherent += currentClusterCount;
                        }
                    }
                    else
                    {
                        if (currentClusterCount >= connectedCount)
                        {
                            vector.Add(currentColor, new CoherencePair(currentClusterCount, 0));
                        }
                        else
                        {
                            vector.Add(currentColor, new CoherencePair(0, currentClusterCount));
                        }
                    }
                }
            }

            double totalPixels = image.GetLength(0) * image.GetLength(1);

            foreach (int key in vector.Keys.ToList())
            {
                vector[key].coherent /= totalPixels;
                vector[key].nonCoherent /= totalPixels;
            }

            return vector;
        }

        public Dictionary<int, CoherenceCenteringPair> generateCoherenceCenteringVector()
        {
            Dictionary<int, CoherenceCenteringPair> vector = new Dictionary<int, CoherenceCenteringPair>();

            startingPointQueue.Add(new Point(0, 0));

            Point currentPoint;
            int currentColor;
            int currentClusterCount;
            bool currentPointisCenter;
            while (startingPointQueue.Count > 0)
            {
                currentPoint = startingPointQueue[0];
                currentColor = image[currentPoint.Y, currentPoint.X];
                startingPointQueue.RemoveAt(0);
                if (currentColor >= 0)
                {
                    currentPointisCenter = currentPoint.Y >= imageYBorder &&
                            currentPoint.Y <= imageHeight - imageYBorder &&
                            currentPoint.X >= imageXBorder &&
                            currentPoint.X <= imageWidth - imageXBorder;

                    currentClusterCount = getClusterCountCentering(currentPoint, currentPointisCenter);
                    if (!vector.ContainsKey(currentColor))
                    {
                        vector.Add(currentColor, new CoherenceCenteringPair(new CenteringPair(0,0), new CenteringPair(0, 0)));
                    }

                    if (currentClusterCount >= connectedCount)
                    {
                         if (currentPointisCenter)
                         {
                             vector[currentColor].coherent.center += currentClusterCount;
                         }
                         else
                         {
                             vector[currentColor].nonCoherent.center += currentClusterCount;
                         }
                     }
                     else if (currentClusterCount < connectedCount)
                     {
                         if(currentPointisCenter)
                         {
                             vector[currentColor].coherent.nonCenter += currentClusterCount;
                         }
                         else
                         {
                             vector[currentColor].nonCoherent.nonCenter += currentClusterCount;
                         }
                     }
                }
            }

            double totalPixels = imageWidth * imageHeight;

            foreach (int key in vector.Keys.ToList())
            {
                vector[key].coherent.center /= totalPixels;
                vector[key].coherent.nonCenter /= totalPixels;
                vector[key].nonCoherent.center /= totalPixels;
                vector[key].nonCoherent.nonCenter /= totalPixels;
            }

            return vector;
        }

        /// <summary>
        /// Function for getting the number of pixels in the cluster where
        /// <code>startPoint</code> is a member and is in center/non-center.
        /// </summary>
        private int getClusterCountCentering(Point startPoint, bool center)
        {
            int totalCount = 0;
            int currentColor = image[startPoint.Y, startPoint.X];
            int[] xvalues;
            int[] yvalues;

            if (settings.EightConnected)
            {
                xvalues = new int[] { -1, 0, 1, -1, 1, -1, 0, 1 };
                yvalues = new int[] { -1, -1, -1, 0, 0, 1, 1, 1 };
            }
            else
            {
                xvalues = new int[] { 0, -1, 1, 0 };
                yvalues = new int[] { -1, 0, 0, 1 };
            }

            List<Point> nextPoints = new List<Point>();
            nextPoints.Add(startPoint);

            Point currentPoint;
            Point newPoint;
            int currentPixelStatus;

            int newX;
            int newY;

            while (nextPoints.Count > 0)
            {
                currentPoint = nextPoints[0];
                image[currentPoint.Y, currentPoint.X] = -1;
                nextPoints.RemoveAt(0);
                totalCount++;

                bool currentIsCenter;
                for (int i = 0; i < xvalues.Length; i++)
                {
                    newX = currentPoint.X + xvalues[i];
                    newY = currentPoint.Y + yvalues[i];
                    currentIsCenter = newY >= imageYBorder &&
                            newY <= imageHeight - imageYBorder &&
                            newX >= imageXBorder &&
                            newX <= imageWidth - imageXBorder;
                    if (currentIsCenter != center) continue;
                    currentPixelStatus = checkPixel(currentPoint.X + xvalues[i], currentPoint.Y + yvalues[i], currentColor);
                    if (currentPixelStatus == 0) continue;
                    else if (currentPixelStatus == 1)
                    {
                        newPoint = new Point(currentPoint.X + xvalues[i], currentPoint.Y + yvalues[i]);
                        if (!startingPointQueue.Contains(newPoint))
                        {
                            startingPointQueue.Add(newPoint);
                        }
                    }
                    else if (currentPixelStatus == 2)
                    {
                        newPoint = new Point(currentPoint.X + xvalues[i], currentPoint.Y + yvalues[i]);
                        if (!nextPoints.Any(p => p.X == newPoint.X && p.Y == newPoint.Y))
                        {
                            nextPoints.Add(newPoint);
                        }
                        else
                        {
                            startingPointQueue.Add(newPoint);
                        }
                    }
                }
            }

            return totalCount;
        }

        /// <summary>
        /// Function for getting the number of pixels in the cluster where
        /// <code>startPoint</code> is a member.
        /// </summary>
        private int getClusterCount(Point startPoint)
        {
            int totalCount = 0;
            int currentColor = image[startPoint.Y, startPoint.X];
            int[] xvalues; 
            int[] yvalues;

            if (settings.EightConnected)
            {
                xvalues = new int[] { -1, 0, 1, -1, 1, -1, 0, 1 };
                yvalues = new int[] { -1, -1, -1, 0, 0, 1, 1, 1 };
            }
            else
            {
                xvalues = new int[] { 0, -1, 1, 0 };
                yvalues = new int[] { -1, 0, 0, 1 };
            }

            List<Point> nextPoints = new List<Point>();
            nextPoints.Add(startPoint);

            Point currentPoint;
            Point newPoint;
            int currentPixelStatus;

            while (nextPoints.Count > 0)
            {
                currentPoint = nextPoints[0];
                image[currentPoint.Y, currentPoint.X] = -1;
                nextPoints.RemoveAt(0);
                totalCount++;
                for (int i = 0; i < xvalues.Length; i++)
                {
                    currentPixelStatus = checkPixel(currentPoint.X + xvalues[i], currentPoint.Y + yvalues[i], currentColor);
                    if (currentPixelStatus == 0) continue;
                    else if (currentPixelStatus == 1)
                    {
                        newPoint = new Point(currentPoint.X + xvalues[i], currentPoint.Y + yvalues[i]);
                        if (!startingPointQueue.Contains(newPoint))
                        {
                            startingPointQueue.Add(newPoint);
                        }
                    }
                    else if (currentPixelStatus == 2)
                    {
                        newPoint = new Point(currentPoint.X + xvalues[i], currentPoint.Y + yvalues[i]);
                        if (!nextPoints.Any(p => p.X == newPoint.X && p.Y == newPoint.Y))
                        {
                            nextPoints.Add(newPoint);
                        }
                    }
                }
            }

            return totalCount;
        }

        /// <summary>
        /// Function for checking a pixel given image coordinates.
        /// </summary>
        /// <param name="checkX">Pixel X coordinate in image</param>
        /// <param name="checkY">Pixel Y coordinate in image</param>
        /// <param name="refColor">Color to compare pixel to</param>
        /// <returns>
        /// 0 - Invalid
        /// 1 - Valid but different color
        /// 2 - Valid and same color
        /// </returns>
        private int checkPixel(int checkX, int checkY, int refColor)
        {
            if (checkX >= 0 && checkX < image.GetLength(1) && checkY >= 0 && checkY < image.GetLength(0) && image[checkY, checkX] >= 0)
            {
                if (image[checkY, checkX] == refColor)
                {
                    return 2;
                }
                else
                {
                    return 1;
                }
            }
            else
            {
                return 0;
            }
        }
    }
}
