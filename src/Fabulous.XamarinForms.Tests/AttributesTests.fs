﻿namespace Fabulous.XamarinForms.Tests

open Fabulous.XamarinForms
open FsCheck
open NUnit.Framework
open FsCheck.NUnit
open Xamarin.Forms

[<TestFixture>]
type ``Small scalar encode tests``() =
    [<Property>]
    member _.``Encoding then decoding a LayoutOptions should return an identical LayoutOptions``(value: LayoutOptions) =
        let encoded = SmallScalars.LayoutOptions.encode value

        let decoded =
            SmallScalars.LayoutOptions.decode encoded

        Assert.AreEqual(value.Alignment, decoded.Alignment)
        Assert.AreEqual(value.Expands, decoded.Expands)

    [<Property>]
    member _.``Encoding then decoding a Color should return an identical Color``(value: Color) =
        let encoded = SmallScalars.Color.encode value
        let decoded = SmallScalars.Color.decode encoded

        // We lose a bit of accuracy when encoding/decoding colors, so we account for that by comparing only on 16 bits
        // Because of a rounding loss, we can't be sure that the values are equal, so we'll just check that they're close by 1 bit
        let assertAsU16 (expected: float) (actual: float) =
            let expectedU16 = uint16(expected * 65535.0)
            let actualU16 = uint16(actual * 65535.0)

            let lowerBound =
                if expectedU16 = 0us then
                    0us
                else
                    expectedU16 - 1us

            Assert.GreaterOrEqual(actualU16, lowerBound)
            Assert.LessOrEqual(actualU16, expectedU16)

        assertAsU16 value.R decoded.R
        assertAsU16 value.G decoded.G
        assertAsU16 value.B decoded.B
        assertAsU16 value.A decoded.A
