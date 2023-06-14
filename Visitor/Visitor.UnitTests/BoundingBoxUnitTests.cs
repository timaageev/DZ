using Visitor;

namespace VisitorUnitTestProject
{
    [TestFixture]
    public class BoundingBoxUnitTests
    {
        [TestCase(1, 2, 3, 2, 1, 3, 5, 5, 4)]
        [TestCase(1, 1, 1, 2, 2, 2, 5, 5, 5)]
        [TestCase(2, 1, 1, 1, 2, 2, 5, 5, 5)]
        public void CompoundBody_Of_TwoCuboids_BoundingBox_Correct(double x0, double y0, double z0, double x1, double y1, double z1, double expectedSizeX, double expectedSizeY, double expectedSizeZ)
        {
            var compoundBody = new CompoundBody(new[]
            {
                new RectangularCuboid(new Vector3D(x0, y0, z0), 4, 4, 4),
                new RectangularCuboid(new Vector3D(x1, y1, z1), 4, 4, 4),
            });
            var boundingBox = compoundBody.TryAcceptVisitor<RectangularCuboid>(new BoundingBoxVisitor());
            Assert.That(boundingBox.SizeX, Is.EqualTo(expectedSizeX), "SizeX");
            Assert.That(boundingBox.SizeY, Is.EqualTo(expectedSizeY), "SizeY");
            Assert.That(boundingBox.SizeZ, Is.EqualTo(expectedSizeZ), "SizeZ");
        }

        [TestCase(0, 0, 0, 1)]
        [TestCase(6, 2, 4, 0)]
        [TestCase(-6, 2, 4, 0)]
        [TestCase(-6, -2, 4, 0)]
        [TestCase(-6, -2, -4, 0)]
        [TestCase(6, 2, -4, 0)]
        [TestCase(6, -2, -4, 0)]
        [TestCase(6, 2, 4, 2)]
        [TestCase(-6, 2, 4, 4)]
        [TestCase(-6, -2, 4, 9)]
        [TestCase(-6, -2, -4, 8)]
        [TestCase(6, 2, -4, 7)]
        [TestCase(6, -2, -4, 1)]
        public void Ball_BoundingBoxVisitor_IsCorrect(double x, double y, double z, double radius)
        {
            var ball = new Ball(new Vector3D(x, y, z), radius);
            var box = ball.TryAcceptVisitor<RectangularCuboid>(new BoundingBoxVisitor());
            var length = radius * 2;
            var expectedBox = new RectangularCuboid(new Vector3D(x, y, z), length, length, length);
            AssertCuboidsEqual(expectedBox, box);
        }

        [TestCase(0, 0, 0, 1, 2, 3)]
        [TestCase(6, 2, 4, 5, 4, 7)]
        [TestCase(-6, 2, 4, 5, 5, 5)]
        [TestCase(-6, -2, 4, 0)]
        [TestCase(-6, -2, -4, 0)]
        [TestCase(6, 2, -4, 0)]
        [TestCase(6, -2, -4, 0)]
        [TestCase(6, 2, 4, 2)]
        [TestCase(-6, 2, 4, 4)]
        [TestCase(-6, -2, 4, 9)]
        [TestCase(-6, -2, -4, 8)]
        [TestCase(6, 2, -4, 7)]
        [TestCase(6, -2, -4, 1)]
        public void RectangularCuboid_BoundingBoxVisitor_IsCorrect(
            double x, double y, double z,
            double length = 4, double width = 7, double height = 5)
        {
            var cuboid = new RectangularCuboid(new Vector3D(x, y, z), length, width, height);
            var box = cuboid.TryAcceptVisitor<RectangularCuboid>(new BoundingBoxVisitor());
            AssertCuboidsEqual(cuboid, box);
        }

        [TestCase(0, 0, 0, 1, 2)]
        [TestCase(6, 2, 4, 5, 4)]
        [TestCase(-6, 2, 4, 5, 5)]
        [TestCase(-6, -2, 4)]
        [TestCase(-6, -2, -4)]
        [TestCase(6, 2, -4)]
        [TestCase(6, -2, -4)]
        [TestCase(6, 2, 4)]
        [TestCase(-6, 2, 4)]
        [TestCase(-6, -2, 4)]
        [TestCase(-6, -2, -4)]
        [TestCase(6, 2, -4)]
        [TestCase(6, -2, -4)]
        public void Cylinder_BoundingBox_IsCorrect(double x, double y, double z, double radius = 4, double height = 5)
        {
            var cylinder = new Cylinder(new Vector3D(x, y, z), height, radius);
            var box = cylinder.TryAcceptVisitor<RectangularCuboid>(new BoundingBoxVisitor());
            var cuboid = new RectangularCuboid
            (
                cylinder.Position,
                cylinder.Radius * 2,
                cylinder.Radius * 2,
                cylinder.SizeZ
            );
            AssertCuboidsEqual(cuboid, box);
        }

        [TestCase(0, 0, 0, 1)]
        [TestCase(0, 0, 0, 6)]
        [TestCase(0, 0, 0, 7)]
        [TestCase(1, 1, 1, 7)]
        [TestCase(2, 3, 6, 5)]
        [TestCase(-2, -3, 6, 3)]
        [TestCase(-12, -23, -46, 13)]
        [TestCase(12, -23, 6, 14)]
        [TestCase(8, 5, -16, 12)]
        public void CompoundBody_BoundingBox_IsCorrect(double x, double y, double z, double radius)
        {
            const int figuresCount = 6;
            const double indent = 2.2;
            var height = radius * 20 + indent * (figuresCount - 1);
            var expectedBox = new RectangularCuboid(new Vector3D(x, y, height / 2 + z), radius * 2, radius * 2, height);

            foreach (var compoundBody in GetCompoundBodies(new Vector3D(x, y, z), radius, indent))
            {
                var box = compoundBody.TryAcceptVisitor<RectangularCuboid>(new BoundingBoxVisitor());
                AssertCuboidsEqual(expectedBox, box);
            }
        }

        public static IEnumerable<CompoundBody> GetCompoundBodies(Vector3D startPosition, double radius, double indent)
        {
            var ball = GetBall(startPosition, radius);
            var cylinder = GetCylinder(ball.Position.CreatePoint(dz: ball.Radius + indent), radius);
            var box = GetRectangularCuboid(cylinder.Position.CreatePoint(dz: cylinder.SizeZ / 2 + indent), radius);
            var compound = GetCompoundBody(box.Position.CreatePoint(dz: box.SizeZ / 2 + indent), radius, indent);
            yield return new CompoundBody(new List<Body> { ball, cylinder, box, compound });


            var cylinder2 = GetCylinder(startPosition, radius);
            var ball2 = GetBall(cylinder2.Position.CreatePoint(dz: cylinder2.SizeZ / 2 + indent), radius);
            var box2 = GetRectangularCuboid(ball2.Position.CreatePoint(dz: ball2.Radius + indent), radius);
            var compound2 = GetCompoundBody(box2.Position.CreatePoint(dz: box2.SizeZ / 2 + indent), radius, indent);
            yield return new CompoundBody(new List<Body> { cylinder2, ball2, box2, compound2 });

            var cylinder3 = GetCylinder(startPosition, radius); //
            var box3 = GetRectangularCuboid(cylinder3.Position.CreatePoint(dz: cylinder3.SizeZ / 2 + indent), radius);
            var ball3 = GetBall(box3.Position.CreatePoint(dz: box3.SizeZ / 2 + indent), radius);
            var compound3 = GetCompoundBody(ball3.Position.CreatePoint(dz: ball3.Radius + indent), radius, indent);
            yield return new CompoundBody(new List<Body> { cylinder3, box3, ball3, compound3 });
        }

        public static Ball GetBall(Vector3D fromPoint, double radius)
        {
            return new Ball(new Vector3D(fromPoint.X, fromPoint.Y, fromPoint.Z + radius), radius);
        }

        public static Cylinder GetCylinder(Vector3D fromPoint, double radius)
        {
            return new Cylinder(new Vector3D(fromPoint.X, fromPoint.Y, fromPoint.Z + radius), radius * 2, radius);
        }

        public static RectangularCuboid GetRectangularCuboid(Vector3D fromPoint, double radius)
        {
            var height = radius * 4;
            return new RectangularCuboid
            (
                new Vector3D(fromPoint.X, fromPoint.Y, fromPoint.Z + height / 2),
                radius * 2,
                radius * 2,
                height
            );
        }

        public static CompoundBody GetCompoundBody(Vector3D fromPoint, double radius, double indent)
        {
            var box1 = GetRectangularCuboid(fromPoint, radius);
            var box2 = GetRectangularCuboid(box1.Position.CreatePoint(dz: box1.SizeZ / 2 + indent), radius);
            var box3 = GetRectangularCuboid(box2.Position.CreatePoint(dz: box2.SizeZ / 2 + indent), radius);
            return new CompoundBody(new List<Body> { box1, box2, box3 });
        }

        private void AssertCuboidsEqual(RectangularCuboid expected, RectangularCuboid actual)
        {
            var message = " is not equal!";
            Assert.IsTrue(expected.Position.Equals(actual.Position, Constants.Inaccuracy), $"{expected.Position} != {actual.Position}");
            Assert.That(actual.SizeX, Is.EqualTo(expected.SizeX).Within(Constants.Inaccuracy), "Length" + message);
            Assert.That(actual.SizeY, Is.EqualTo(expected.SizeY).Within(Constants.Inaccuracy), "Width" + message);
            Assert.That(actual.SizeZ, Is.EqualTo(expected.SizeZ).Within(Constants.Inaccuracy), "Height" + message);
        }
    }
}
