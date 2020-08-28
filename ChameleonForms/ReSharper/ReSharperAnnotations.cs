using System;
#pragma warning disable 1591
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable IntroduceOptionalParameters.Global
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable InconsistentNaming

// ReSharper disable once CheckNamespace
namespace JetBrains.Annotations
{
    /// <summary>
    /// Indicates that the value of the marked element could never be <c>null</c>
    /// </summary>
    /// <example><code>
    /// [NotNull] public object Foo() {
    ///   return null; // Warning: Possible 'null' assignment
    /// }
    /// </code></example>
    [AttributeUsage(
        AttributeTargets.Method | AttributeTargets.Parameter |
        AttributeTargets.Property | AttributeTargets.Delegate |
        AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    internal sealed class NotNullAttribute : Attribute { }

    /// <summary>
    /// Indicates that a parameter is a path to a file or a folder
    /// within a web project. Path can be relative or absolute,
    /// starting from web root (~)
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    internal class PathReferenceAttribute : Attribute
    {
        public PathReferenceAttribute() { }
        public PathReferenceAttribute([PathReference] string basePath)
        {
            BasePath = basePath;
        }

        [NotNull]
        public string BasePath { get; private set; }
    }

    /// <summary>
    /// ASP.NET MVC attribute. If applied to a parameter, indicates that
    /// the parameter is an MVC partial view. If applied to a method,
    /// the MVC partial view name is calculated implicitly from the context.
    /// Use this attribute for custom wrappers similar to
    /// <c>System.Web.Mvc.Html.RenderPartialExtensions.RenderPartial(HtmlHelper, String)</c>
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    internal sealed class AspMvcPartialViewAttribute : PathReferenceAttribute { }
}