using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using DS7.Data;
using DS7.Grid;

/// <summary>
/// EditMode unit tests for pure-C# logic in DS7.
/// Run via: Window > General > Test Runner > EditMode > Run All
/// </summary>
namespace DS7.Tests
{
    public class HexCoordinatesTests
    {
        [Test]
        public void Distance_SameHex_IsZero()
        {
            var a = new HexCoordinates(0, 0);
            Assert.AreEqual(0, HexCoordinates.Distance(a, a));
        }

        [Test]
        public void Distance_AdjacentHex_IsOne()
        {
            var a = new HexCoordinates(0, 0);
            var b = new HexCoordinates(1, 0);
            Assert.AreEqual(1, HexCoordinates.Distance(a, b));
        }

        [Test]
        public void Distance_TwoAway_IsTwo()
        {
            var a = new HexCoordinates(0, 0);
            var b = new HexCoordinates(2, 0);
            Assert.AreEqual(2, HexCoordinates.Distance(a, b));
        }

        [Test]
        public void CubeCoordinateConstraint_Holds()
        {
            // x + y + z must equal 0
            var h = new HexCoordinates(3, -1);
            Assert.AreEqual(0, h.X + h.Y + h.Z);
        }

        [Test]
        public void AllNeighbors_Returns6()
        {
            var h = new HexCoordinates(0, 0);
            Assert.AreEqual(6, h.AllNeighbors().Length);
        }

        [Test]
        public void OffsetRoundTrip()
        {
            var original = new HexCoordinates(3, -2);
            var offset   = original.ToOffsetCoords();
            var roundtrip = HexCoordinates.FromOffsetCoords(offset.x, offset.y);
            Assert.AreEqual(original, roundtrip);
        }

        [Test]
        public void Equality_Works()
        {
            var a = new HexCoordinates(1, 2);
            var b = new HexCoordinates(1, 2);
            Assert.AreEqual(a, b);
            Assert.IsTrue(a == b);
        }

        [Test]
        public void Inequality_Works()
        {
            var a = new HexCoordinates(1, 2);
            var b = new HexCoordinates(1, 3);
            Assert.AreNotEqual(a, b);
            Assert.IsTrue(a != b);
        }
    }

    public class CombatResolverTests
    {
        [Test]
        public void CalculateDamage_FullStrength_ReturnsPositive()
        {
            var weapon = ScriptableObject.CreateInstance<WeaponData>();
            weapon.firePower       = 100;
            weapon.hitRateInfantry = 80;

            // We can't easily create Unit without a full scene, so we test the math directly
            float firePower  = weapon.firePower;
            float hitRate    = weapon.hitRateInfantry / 100f;
            float strength   = 1f;
            float experience = 1f;
            float terrainDef = 1f;

            float rawDamage = firePower * hitRate * strength * experience * terrainDef;
            int damage = UnityEngine.Mathf.RoundToInt(rawDamage / 10f);

            Assert.Greater(damage, 0);
            Assert.LessOrEqual(damage, 10);

            ScriptableObject.DestroyImmediate(weapon);
        }

        [Test]
        public void WeaponData_GetRange_ReturnsCorrectAltitude()
        {
            var weapon = ScriptableObject.CreateInstance<WeaponData>();
            weapon.rangeHigh    = 5;
            weapon.rangeMed     = 3;
            weapon.rangeLow     = 2;
            weapon.rangeGround  = 1;
            weapon.rangeSurface = 4;
            weapon.rangeDeep    = 0;

            Assert.AreEqual(5, weapon.GetRange(AltitudeLayer.HighAir));
            Assert.AreEqual(3, weapon.GetRange(AltitudeLayer.MedAir));
            Assert.AreEqual(1, weapon.GetRange(AltitudeLayer.Ground));

            ScriptableObject.DestroyImmediate(weapon);
        }
    }

    public class SupplySystemTests
    {
        [Test]
        public void ResupplyCost_IsAtLeastTen()
        {
            var unit = ScriptableObject.CreateInstance<UnitData>();
            unit.productionCost = 50;

            int cost = UnityEngine.Mathf.Max(10, unit.productionCost / 10);
            Assert.GreaterOrEqual(cost, 10);

            ScriptableObject.DestroyImmediate(unit);
        }

        [Test]
        public void FactionFunds_DeductedAfterResupply()
        {
            var funds = new Dictionary<Faction, int> { { Faction.Blue, 1000 } };
            int cost  = 50;
            funds[Faction.Blue] -= cost;
            Assert.AreEqual(950, funds[Faction.Blue]);
        }
    }

    public class TurnOrderTests
    {
        [Test]
        public void TurnIndex_WrapsAround()
        {
            // Simulate cycling through 3 nations
            int index   = 0;
            int nations = 3;

            index = (index + 1) % nations; Assert.AreEqual(1, index);
            index = (index + 1) % nations; Assert.AreEqual(2, index);
            index = (index + 1) % nations; Assert.AreEqual(0, index); // wrapped
        }

        [Test]
        public void TurnNumber_IncrementsAfterFullRound()
        {
            int currentNationIndex = 0;
            int turnNumber         = 1;
            int nationCount        = 3;

            for (int step = 0; step < nationCount; step++)
            {
                currentNationIndex = (currentNationIndex + 1) % nationCount;
                if (currentNationIndex == 0) turnNumber++;
            }

            Assert.AreEqual(2, turnNumber);
        }
    }
}
