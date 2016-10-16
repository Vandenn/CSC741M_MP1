using ColorMine.ColorSpaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSC741M_MP1.Algorithms.Helpers
{
    public class CoherenceCalculator
    {
        private int[,] image;
        private bool eightConnected;
        private int connectedCount;
        private List<Point> startingPointQueue;

        public CoherenceCalculator(Luv[,] rawImage, double connectednessThreshold = 0.01, bool eightConnected = false)
        {
            this.eightConnected = eightConnected;
            image = convertImageToLuvIndex(rawImage);
            connectedCount = (int)Math.Floor(image.GetLength(0) * image.GetLength(1) * connectednessThreshold);
            startingPointQueue = new List<Point>();
        }

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

        private int getClusterCount(Point startPoint)
        {
            int totalCount = 0;
            int currentColor = image[startPoint.Y, startPoint.X];
            int[] xvalues; 
            int[] yvalues;

            if (eightConnected)
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

        // 0 - Invalid
        // 1 - Valid but different Color
        // 2 - Valid and same color
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

    public class CoherenceCalculator2
    {
        private class CoherenceData
        {
            public bool[,] traversed { get; set; }
            public Dictionary<int, CoherencePair> vector { get; set; }
            public CoherenceData(int imageWidth, int imageHeight)
            {
                traversed = new bool[imageHeight, imageWidth];
                vector = new Dictionary<int, CoherencePair>();
            }
        }

        private Luv[,] rawImage;
        private int[,] image;
        private CoherenceData data;
        private bool eightConnected;
        private int connectedCount;

        public CoherenceCalculator2(Luv[,] rawImage, double connectednessThreshold = 0.01, bool eightConnected = false)
        {
            this.rawImage = rawImage;
            this.eightConnected = eightConnected;
            data = new CoherenceData(rawImage.GetLength(1), rawImage.GetLength(0));
            image = convertImageToLuvIndex(rawImage);
            connectedCount = (int) Math.Floor(image.GetLength(0) * image.GetLength(1) / connectednessThreshold);
        }

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
            calculateConnectedness(
                0,
                0,
                image[0, 0],
                0);
            Dictionary<int, CoherencePair> vector = data.vector;

            double totalPixels = image.GetLength(0) * image.GetLength(1);

            foreach (int key in vector.Keys.ToList())
            {
                vector[key].coherent /= totalPixels;
                vector[key].nonCoherent /= totalPixels;
            }

            return vector;
        }

        private int calculateConnectedness(int x, int y, int currentColor, double currentCount)
        {
            data.traversed[y, x] = true;
            bool firstOccurrence = currentCount == 0;

            bool xp1 = checkIfInBoundsHorizontal(image.GetLength(1), x + 1);
            bool xm1 = checkIfInBoundsHorizontal(image.GetLength(1), x - 1);
            bool yp1 = checkIfInBoundsVertical(image.GetLength(0), y + 1);
            bool ym1 = checkIfInBoundsVertical(image.GetLength(0), y - 1);

            if (xp1)
            {
                currentCount += nextCoordinateForConnectedness(x + 1, y, currentColor, currentCount);
                if (eightConnected)
                {
                    if (yp1)
                    {
                        currentCount += nextCoordinateForConnectedness(x + 1, y + 1, currentColor, currentCount);
                    }
                    if (ym1)
                    {
                        currentCount += nextCoordinateForConnectedness(x + 1, y - 1, currentColor, currentCount);
                    }
                }
            }
            if (xm1)
            {
                currentCount += nextCoordinateForConnectedness(x - 1, y, currentColor, currentCount);
                if (eightConnected)
                {
                    if (yp1)
                    {
                        currentCount += nextCoordinateForConnectedness(x - 1, y + 1, currentColor, currentCount);
                    }
                    if (ym1)
                    {
                        currentCount += nextCoordinateForConnectedness(x - 1, y - 1, currentColor, currentCount);
                    }
                }
            }
            if (yp1)
            {
                currentCount += nextCoordinateForConnectedness(x, y + 1, currentColor, currentCount);
            }
            if (ym1)
            {
                currentCount += nextCoordinateForConnectedness(x, y + 1, currentColor, currentCount);
            }

            if (firstOccurrence)
            {
                if (currentCount >= connectedCount)
                {
                    if (data.vector.ContainsKey(currentColor))
                    {
                        data.vector[currentColor].coherent += currentCount;
                    }
                    else
                    {
                        data.vector.Add(currentColor, new CoherencePair(currentCount, 0));
                    }
                }
                else
                {
                    if (data.vector.ContainsKey(currentColor))
                    {
                        data.vector[currentColor].nonCoherent += currentCount;
                    }
                    else
                    {
                        data.vector.Add(currentColor, new CoherencePair(0, currentCount));
                    }
                }
            }

            return Convert.ToInt32(currentCount);
        }

        private int nextCoordinateForConnectedness(int x, int y, int currentColor, double currentCount)
        {
            if (data.traversed[y, x] == false)
            {
                if (image[y, x] == currentColor)
                {
                    return calculateConnectedness(x, y, currentColor, currentCount + 1);
                }
                else
                {
                    calculateConnectedness(x, y, image[y, x], 0);
                }
            }
            return 0;
        }

        private bool checkIfInBoundsVertical(int imageHeight, int y)
        {
            return y >= 0 && y < imageHeight;
        }

        private bool checkIfInBoundsHorizontal(int imageWidth, int x)
        {
            return x >= 0 && x < imageWidth;
        }
    }
}
