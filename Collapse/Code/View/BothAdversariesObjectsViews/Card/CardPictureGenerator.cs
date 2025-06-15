using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Collapse.Code.Model;

namespace Collapse.Code.View.BothAdversariesObjectsViews.Card
{
    public static class CardPictureGenerator
    {
        private enum SimpleElementType { Circle, Rectangle, Triangle, Star, Cross, Diamond, Crescent, Heart, Bolt, Hexagon }
        private enum ComplexElementType { Polygon, Gear, Wave, Spiral, Flower, Infinity, Atom, Snowflake, Mandala, CelticKnot }

        private const byte BaseElementCount = 8;
        private const byte MaxExtraElements = 6;

        private const byte MinElementSize = 15;
        private const byte MaxElementSizeDivider = 3;
        private const double ComplexityThreshold = 0.75;
        private const byte MinComplexElementIndex = 2;

        private const byte CircleSegments = 36;

        private const byte CurveSegments = 24;

        private const byte StarMinPoints = 5;
        private const byte StarMaxExtraPoints = 4;

        private const byte GearMinTeeth = 6;
        private const byte GearMaxExtraTeeth = 4;

        private const byte WaveMinCycles = 2;
        private const byte WaveMaxCycles = 5;

        private const byte PolygonMinSides = 3;
        private const byte PolygonMaxSides = 8;

        private const byte WaveSamples = 20;

        private const byte SpiralTurns = 3;

        private const byte FlowerPetals = 6;

        private const float CrescentRatio = 0.7f;

        private const double FirstGearAngleRatio = 0.3;
        private const double SecondGearAngleRatio = 0.7;

        private const byte RectangleVertexQuantity = 4;
        private const byte TriangleVertexQuantity = 3;

        private static Texture2D _pixelTexture;
        private static GraphicsDevice _graphicsDevice;

        private static readonly Color[] _allowedColors =
        {
            new Color(255, 96, 96),
            new Color(96, 96, 255),
            new Color(255, 255, 255),
            new Color(96, 96, 96),
        };

        public static void Load(GraphicsDevice graphicsDevice)
        {
            _graphicsDevice = graphicsDevice;
            _pixelTexture = new Texture2D(graphicsDevice, 1, 1);
            _pixelTexture.SetData(new[] { Color.White });
        }


        public static Texture2D GenerateCardPicture(int width, int height)
        {
            var renderTarget = new RenderTarget2D(_graphicsDevice, width, height);
            _graphicsDevice.SetRenderTarget(renderTarget);
            _graphicsDevice.Clear(Color.Transparent);

            using (var spriteBatch = new SpriteBatch(_graphicsDevice))
            {
                spriteBatch.Begin();

                var elementCount = BaseElementCount + GameModel.Random.Next(MaxExtraElements);

                for (var i = 0; i < elementCount; i++)
                {
                    var color = _allowedColors[GameModel.Random.Next(_allowedColors.Length)];
                    var size = MinElementSize + GameModel.Random.Next(Math.Min(width, height) / MaxElementSizeDivider);
                    var x = GameModel.Random.Next(width);
                    var y = GameModel.Random.Next(height);
                    var rotation = (float)GameModel.Random.NextDouble() * MathHelper.TwoPi;
                    var thickness = 1 + GameModel.Random.Next(4);

                    if (GameModel.Random.NextDouble() > ComplexityThreshold && i > MinComplexElementIndex)
                        DrawComplexElement(spriteBatch, x, y, size, color, rotation, thickness);
                    else
                        DrawSimpleElement(spriteBatch, x, y, size, color, rotation, thickness);
                }

                spriteBatch.End();
            }

            _graphicsDevice.SetRenderTarget(null);
            return renderTarget;
        }

        private static void DrawLine(SpriteBatch spriteBatch, Vector2 start, Vector2 end, float thickness, Color color)
        {
            var length = Vector2.Distance(start, end);
            var angle = (float)Math.Atan2(end.Y - start.Y, end.X - start.X);

            spriteBatch.Draw(_pixelTexture,
                start,
                null,
                color,
                angle,
                Vector2.Zero,
                new Vector2(length, thickness),
                SpriteEffects.None,
                0);
        }

        private static void DrawSimpleElement(SpriteBatch spriteBatch, int x, int y, int size, Color color, float rotation, float thickness)
        {
            var elementType = (SimpleElementType)GameModel.Random.Next(Enum.GetValues(typeof(SimpleElementType)).Length);

            switch (elementType)
            {
                case SimpleElementType.Circle:
                    DrawCircle(spriteBatch, x, y, size / 2, color, thickness);
                    break;
                case SimpleElementType.Rectangle:
                    DrawRotatedRectangle(spriteBatch, x, y, size, size, color, rotation, thickness);
                    break;
                case SimpleElementType.Triangle:
                    DrawTriangle(spriteBatch, x, y, size, color, rotation, thickness);
                    break;
                case SimpleElementType.Star:
                    DrawStar(spriteBatch, x, y, size / 2, size / 4, StarMinPoints + GameModel.Random.Next(StarMaxExtraPoints), color, thickness);
                    break;
                case SimpleElementType.Cross:
                    DrawCross(spriteBatch, x, y, size, color, thickness, rotation);
                    break;
                case SimpleElementType.Diamond:
                    DrawDiamond(spriteBatch, x, y, size, color, rotation, thickness);
                    break;
                case SimpleElementType.Crescent:
                    DrawCrescent(spriteBatch, x, y, size / 2, color, rotation, thickness);
                    break;
                case SimpleElementType.Heart:
                    DrawHeart(spriteBatch, x, y, size, color, thickness);
                    break;
                case SimpleElementType.Bolt:
                    DrawBolt(spriteBatch, x, y, size, color, thickness);
                    break;
                case SimpleElementType.Hexagon:
                    DrawPolygon(spriteBatch, x, y, size / 2, 6, color, rotation, thickness);
                    break;
            }
        }

        private static void DrawComplexElement(SpriteBatch spriteBatch, int x, int y, int size, Color color, float rotation, float thickness)
        {
            var elementType = (ComplexElementType)GameModel.Random.Next(Enum.GetValues(typeof(ComplexElementType)).Length);

            switch (elementType)
            {
                case ComplexElementType.Polygon:
                    DrawPolygon(spriteBatch, x, y, size / 2, PolygonMinSides + GameModel.Random.Next(PolygonMaxSides - PolygonMinSides + 1), color, rotation, thickness);
                    break;
                case ComplexElementType.Gear:
                    DrawGear(spriteBatch, x, y, size / 2, size / 4, GearMinTeeth + GameModel.Random.Next(GearMaxExtraTeeth), color, thickness, rotation);
                    break;
                case ComplexElementType.Wave:
                    DrawWave(spriteBatch, x, y, size, color, thickness, WaveMinCycles + GameModel.Random.Next(WaveMaxCycles - WaveMinCycles + 1));
                    break;
                case ComplexElementType.Spiral:
                    DrawSpiral(spriteBatch, x, y, size / 2, color, thickness, rotation);
                    break;
                case ComplexElementType.Flower:
                    DrawFlower(spriteBatch, x, y, size / 2, color, thickness, rotation);
                    break;
                case ComplexElementType.Infinity:
                    DrawInfinity(spriteBatch, x, y, size / 2, color, thickness, rotation);
                    break;
                case ComplexElementType.Atom:
                    DrawAtom(spriteBatch, x, y, size, color, thickness);
                    break;
                case ComplexElementType.Snowflake:
                    DrawSnowflake(spriteBatch, x, y, size, color, thickness);
                    break;
                case ComplexElementType.Mandala:
                    DrawMandala(spriteBatch, x, y, size / 2, color, thickness, rotation);
                    break;
                case ComplexElementType.CelticKnot:
                    DrawCelticKnot(spriteBatch, x, y, size / 2, color, thickness, rotation);
                    break;
            }
        }

        private static void DrawCircle(SpriteBatch spriteBatch, int x, int y, float radius, Color color, float thickness)
        {
            var points = new Vector2[CircleSegments];

            for (var i = 0; i < CircleSegments; i++)
            {
                var angle = MathHelper.TwoPi * i / CircleSegments;
                points[i] = new Vector2(x + radius * (float)Math.Cos(angle), y + radius * (float)Math.Sin(angle));
            }

            for (var i = 0; i < CircleSegments; i++)
            {
                var next = (i + 1) % CircleSegments;
                DrawLine(spriteBatch, points[i], points[next], thickness, color);
            }
        }

        private static void DrawRotatedRectangle(SpriteBatch spriteBatch, int x, int y, int width, int height, Color color, float rotation, float thickness)
        {
            var halfWidth = width / 2;
            var halfHeight = height / 2;

            var corners = new Vector2[RectangleVertexQuantity];
            corners[0] = new Vector2(-halfWidth, -halfHeight);
            corners[1] = new Vector2(halfWidth, -halfHeight);
            corners[2] = new Vector2(halfWidth, halfHeight);
            corners[3] = new Vector2(-halfWidth, halfHeight);

            for (var i = 0; i < RectangleVertexQuantity; i++)
            {
                var rotatedCorner = RotateVector(corners[i], rotation);
                var nextRotatedCorner = RotateVector(corners[(i + 1) % RectangleVertexQuantity], rotation);
                DrawLine(spriteBatch, new Vector2(x, y) + rotatedCorner, new Vector2(x, y) + nextRotatedCorner, thickness, color);
            }
        }

        private static void DrawTriangle(SpriteBatch spriteBatch, int x, int y, int size, Color color, float rotation, float thickness)
        {
            var points = new Vector2[TriangleVertexQuantity];
            points[0] = new Vector2(0, -size / 2);
            points[1] = new Vector2(size / 2, size / 2);
            points[2] = new Vector2(-size / 2, size / 2);

            for (var i = 0; i < TriangleVertexQuantity; i++)
                points[i] = RotateVector(points[i], rotation) + new Vector2(x, y);

            for (var i = 0; i < TriangleVertexQuantity; i++)
                DrawLine(spriteBatch, points[i], points[(i + 1) % TriangleVertexQuantity], thickness, color);
        }

        private static void DrawStar(SpriteBatch spriteBatch, int x, int y, float outerRadius, float innerRadius, int points, Color color, float thickness)
        {
            var starPoints = new Vector2[points * 2];
            var angleIncrement = MathHelper.TwoPi / points;

            for (var i = 0; i < points; i++)
            {
                var outerAngle = i * angleIncrement;
                var innerAngle = outerAngle + angleIncrement / 2;
                starPoints[i * 2] = new Vector2(x + outerRadius * (float)Math.Cos(outerAngle), y + outerRadius * (float)Math.Sin(outerAngle));
                starPoints[i * 2 + 1] = new Vector2(x + innerRadius * (float)Math.Cos(innerAngle), y + innerRadius * (float)Math.Sin(innerAngle));
            }

            for (var i = 0; i < points * 2; i++)
                DrawLine(spriteBatch, starPoints[i], starPoints[(i + 1) % (points * 2)], thickness, color);
        }

        private static void DrawDiamond(SpriteBatch spriteBatch, int x, int y, int size, Color color, float rotation, float thickness)
        {
            var halfSize = size / 2;
            var points = new Vector2[RectangleVertexQuantity];
            points[0] = new Vector2(0, -halfSize);
            points[1] = new Vector2(halfSize, 0);
            points[2] = new Vector2(0, halfSize);
            points[3] = new Vector2(-halfSize, 0);

            for (var i = 0; i < RectangleVertexQuantity; i++)
                points[i] = RotateVector(points[i], rotation) + new Vector2(x, y);

            for (var i = 0; i < RectangleVertexQuantity; i++)
                DrawLine(spriteBatch, points[i], points[(i + 1) % RectangleVertexQuantity], thickness, color);
        }

        private static void DrawCrescent(SpriteBatch spriteBatch, int x, int y, float radius, Color color, float rotation, float thickness)
        {
            var points = new Vector2[CircleSegments * 2];
            var smallRadius = radius * CrescentRatio;

            for (var i = 0; i < CircleSegments; i++)
            {
                var angle = MathHelper.TwoPi * i / CircleSegments;
                points[i] = new Vector2(x + radius * (float)Math.Cos(angle), y + radius * (float)Math.Sin(angle));
                points[i + CircleSegments] = new Vector2(x + smallRadius * (float)Math.Cos(angle + MathHelper.Pi), y + smallRadius * (float)Math.Sin(angle + MathHelper.Pi));
            }

            for (var i = 0; i < CircleSegments; i++)
            {
                DrawLine(spriteBatch, points[i], points[(i + 1) % CircleSegments], thickness, color);
                DrawLine(spriteBatch, points[i + CircleSegments], points[((i + 1) % CircleSegments) + CircleSegments], thickness, color);
            }
        }

        private static void DrawHeart(SpriteBatch spriteBatch, int x, int y, float size, Color color, float thickness)
        {
            var points = new Vector2[CurveSegments];
            for (var i = 0; i < CurveSegments; i++)
            {
                var t = (float)i / (CurveSegments - 1);
                var angle = t * MathHelper.TwoPi;
                var xHeart = 16 * (float)Math.Pow(Math.Sin(angle), 3);
                var yHeart = -(13 * (float)Math.Cos(angle) - 5 * (float)Math.Cos(2 * angle) - 2 * (float)Math.Cos(3 * angle) - (float)Math.Cos(4 * angle));
                points[i] = new Vector2(x + xHeart * size / 16f, y + yHeart * size / 16f);
            }

            for (var i = 0; i < CurveSegments - 1; i++)
                DrawLine(spriteBatch, points[i], points[i + 1], thickness, color);
        }

        private static void DrawBolt(SpriteBatch spriteBatch, int x, int y, float size, Color color, float thickness)
        {
            var points = new Vector2[7];
            points[0] = new Vector2(x, y - size / 2);
            points[1] = new Vector2(x + size / 3, y - size / 6);
            points[2] = new Vector2(x - size / 3, y + size / 6);
            points[3] = new Vector2(x + size / 3, y + size / 2);
            points[4] = new Vector2(x + size / 3, y + size / 6);
            points[5] = new Vector2(x - size / 3, y - size / 6);
            points[6] = points[0];

            for (var i = 0; i < points.Length - 1; i++)
                DrawLine(spriteBatch, points[i], points[i + 1], thickness, color);
        }

        private static void DrawPolygon(SpriteBatch spriteBatch, int x, int y, float radius, int sides, Color color, float rotation, float thickness)
        {
            var points = new Vector2[sides];
            var angleIncrement = MathHelper.TwoPi / sides;

            for (var i = 0; i < sides; i++)
            {
                var angle = i * angleIncrement + rotation;
                points[i] = new Vector2(x + radius * (float)Math.Cos(angle), y + radius * (float)Math.Sin(angle));
            }

            for (var i = 0; i < sides; i++)
                DrawLine(spriteBatch, points[i], points[(i + 1) % sides], thickness, color);
        }

        private static void DrawCross(SpriteBatch spriteBatch, int x, int y, int size, Color color, float thickness, float rotation)
        {
            var halfSize = size / 2;
            var horizontalStart = RotateVector(new Vector2(-halfSize, 0), rotation) + new Vector2(x, y);
            var horizontalEnd = RotateVector(new Vector2(halfSize, 0), rotation) + new Vector2(x, y);
            var verticalStart = RotateVector(new Vector2(0, -halfSize), rotation) + new Vector2(x, y);
            var verticalEnd = RotateVector(new Vector2(0, halfSize), rotation) + new Vector2(x, y);

            DrawLine(spriteBatch, horizontalStart, horizontalEnd, thickness, color);
            DrawLine(spriteBatch, verticalStart, verticalEnd, thickness, color);
        }

        private static void DrawGear(SpriteBatch spriteBatch, int x, int y, float outerRadius, float innerRadius, int teeth, Color color, float thickness, float rotation)
        {
            var points = new List<Vector2>();
            var angleIncrement = MathHelper.TwoPi / teeth;

            for (var i = 0; i < teeth; i++)
            {
                var firstAngle = i * angleIncrement + rotation;
                var secondAngle = firstAngle + angleIncrement * FirstGearAngleRatio;
                var thirdAngle = firstAngle + angleIncrement * SecondGearAngleRatio;

                points.Add(new Vector2(x + outerRadius * (float)Math.Cos(firstAngle), y + outerRadius * (float)Math.Sin(firstAngle)));
                points.Add(new Vector2(x + innerRadius * (float)Math.Cos(secondAngle), y + innerRadius * (float)Math.Sin(secondAngle)));
                points.Add(new Vector2(x + innerRadius * (float)Math.Cos(thirdAngle), y + innerRadius * (float)Math.Sin(thirdAngle)));
            }

            for (var i = 0; i < points.Count; i++)
                DrawLine(spriteBatch, points[i], points[(i + 1) % points.Count], thickness, color);
        }

        private static void DrawWave(SpriteBatch spriteBatch, int x, int y, int size, Color color, float thickness, int waves)
        {
            var points = new Vector2[WaveSamples];
            var waveHeight = size / 4;

            for (var i = 0; i < WaveSamples; i++)
            {
                var t = (float)i / (WaveSamples - 1);
                var waveX = x - size / 2 + t * size;
                var waveY = y + waveHeight * (float)Math.Sin(t * MathHelper.TwoPi * waves);
                points[i] = new Vector2(waveX, waveY);
            }

            for (var i = 0; i < WaveSamples - 1; i++)
                DrawLine(spriteBatch, points[i], points[i + 1], thickness, color);
        }

        private static void DrawSpiral(SpriteBatch spriteBatch, int x, int y, float radius, Color color, float thickness, float rotation)
        {
            var points = new Vector2[CircleSegments * SpiralTurns];
            var angleIncrement = MathHelper.TwoPi / CircleSegments;

            for (var i = 0; i < points.Length; i++)
            {
                var t = (float)i / points.Length;
                var angle = i * angleIncrement + rotation;
                var currentRadius = radius * t;
                points[i] = new Vector2(x + currentRadius * (float)Math.Cos(angle), y + currentRadius * (float)Math.Sin(angle));
            }

            for (var i = 0; i < points.Length - 1; i++)
                DrawLine(spriteBatch, points[i], points[i + 1], thickness, color);
        }

        private static void DrawFlower(SpriteBatch spriteBatch, int x, int y, float radius, Color color, float thickness, float rotation)
        {
            var points = new Vector2[CircleSegments];
            var petalSize = radius * 0.4f;

            for (var i = 0; i < CircleSegments; i++)
            {
                var angle = MathHelper.TwoPi * i / CircleSegments + rotation;
                var petalOffset = petalSize * (float)Math.Sin(angle * FlowerPetals);
                var currentRadius = radius + petalOffset;
                points[i] = new Vector2(x + currentRadius * (float)Math.Cos(angle), y + currentRadius * (float)Math.Sin(angle));
            }

            for (var i = 0; i < CircleSegments; i++)
                DrawLine(spriteBatch, points[i], points[(i + 1) % CircleSegments], thickness, color);
        }

        private static void DrawInfinity(SpriteBatch spriteBatch, int x, int y, float size, Color color, float thickness, float rotation)
        {
            var points = new Vector2[CircleSegments];
            var loopSize = size * 0.5f;

            for (var i = 0; i < CircleSegments; i++)
            {
                var angle = MathHelper.TwoPi * i / CircleSegments + rotation;
                var tx = (float)Math.Sin(angle);
                var ty = (float)Math.Sin(angle * 2);
                points[i] = new Vector2(x + loopSize * tx, y + loopSize * ty);
            }

            for (var i = 0; i < CircleSegments; i++)
                DrawLine(spriteBatch, points[i], points[(i + 1) % CircleSegments], thickness, color);
        }

        private static void DrawAtom(SpriteBatch spriteBatch, int x, int y, float size, Color color, float thickness)
        {
            DrawCircle(spriteBatch, x, y, size / 8f, color, thickness);

            for (var i = 0; i < 3; i++)
            {
                var orbitSize = size * (0.3f + i * 0.2f);
                DrawCircle(spriteBatch, x, y, orbitSize, color, thickness / 2f);
            }
        }

        private static void DrawSnowflake(SpriteBatch spriteBatch, int x, int y, float size, Color color, float thickness)
        {
            for (var i = 0; i < 6; i++)
            {
                var angle = i * MathHelper.TwoPi / 6;
                var end = new Vector2(x + size * (float)Math.Cos(angle), y + size * (float)Math.Sin(angle));
                DrawLine(spriteBatch, new Vector2(x, y), end, thickness, color);

                for (int j = 1; j <= 2; j++)
                {
                    var branchAngle = angle + (j % 2 == 0 ? -1 : 1) * MathHelper.PiOver4;
                    var branchEnd = new Vector2(x + size * 0.5f * (float)Math.Cos(branchAngle), y + size * 0.5f * (float)Math.Sin(branchAngle));
                    DrawLine(spriteBatch, end, branchEnd, thickness * 0.8f, color);
                }
            }
        }

        private static void DrawMandala(SpriteBatch spriteBatch, int x, int y, float radius, Color color, float thickness, float rotation)
        {
            var layers = 3 + GameModel.Random.Next(3);
            for (int i = 0; i < layers; i++)
            {
                var layerRadius = radius * (0.2f + 0.25f * i);
                var petals = 4 + i * 2;
                DrawFlower(spriteBatch, x, y, layerRadius * 2, color, thickness * (1 - i * 0.2f), rotation + i * 0.1f);
            }
        }

        private static void DrawCelticKnot(SpriteBatch spriteBatch, int x, int y, float size, Color color, float thickness, float rotation)
        {
            var points = new Vector2[CircleSegments * 2];
            var loopSize = size * 0.7f;

            for (var i = 0; i < CircleSegments; i++)
            {
                var angle = MathHelper.TwoPi * i / CircleSegments + rotation;
                var tx = (float)Math.Sin(angle * 3);
                var ty = (float)Math.Cos(angle * 2);
                points[i] = new Vector2(x + loopSize * tx, y + loopSize * ty);
                points[i + CircleSegments] = new Vector2(x + loopSize * ty, y + loopSize * tx);
            }

            for (var i = 0; i < CircleSegments; i++)
            {
                DrawLine(spriteBatch, points[i], points[(i + 1) % CircleSegments], thickness, color);
                DrawLine(spriteBatch, points[i + CircleSegments], points[((i + 1) % CircleSegments) + CircleSegments], thickness, color);
            }
        }

        private static Vector2 RotateVector(Vector2 vector, float angle)
        {
            var cos = (float)Math.Cos(angle);
            var sin = (float)Math.Sin(angle);
            return new Vector2(vector.X * cos - vector.Y * sin, vector.X * sin + vector.Y * cos);
        }
    }
}
