﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editor.CSharp.Outlining;
using Microsoft.CodeAnalysis.Editor.Implementation.Outlining;
using Roslyn.Test.Utilities;
using Xunit;

namespace Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.Outlining
{
    public class DestructorDeclarationOutlinerTests : AbstractCSharpSyntaxNodeOutlinerTests<DestructorDeclarationSyntax>
    {
        internal override AbstractSyntaxOutliner CreateOutliner() => new DestructorDeclarationOutliner();

        [Fact, Trait(Traits.Feature, Traits.Features.Outlining)]
        public async Task TestDestructor()
        {
            const string code = @"
class C
{
    {|hint:$$~C(){|collapse:
    {
    }|}|}
}";

            await VerifyRegionsAsync(code,
                Region("collapse", "hint", CSharpOutliningHelpers.Ellipsis, autoCollapse: true));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Outlining)]
        public async Task TestDestructorWithComments()
        {
            const string code = @"
class C
{
    {|span1:// Foo
    // Bar|}
    {|hint2:$$~C(){|collapse2:
    {
    }|}|}
}";

            await VerifyRegionsAsync(code,
                Region("span1", "// Foo ...", autoCollapse: true),
                Region("collapse2", "hint2", CSharpOutliningHelpers.Ellipsis, autoCollapse: true));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Outlining)]
        public async Task TestDestructorMissingCloseParenAndBody()
        {
            // Expected behavior is that the class should be outlined, but the destructor should not.

            const string code = @"
class C
{
    $$~C(
}";

            await VerifyNoRegionsAsync(code);
        }
    }
}
