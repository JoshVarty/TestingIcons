using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;
using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using System.Windows.Media;

namespace TestingIconsAgainstVS
{
    [ContentType("CSharp")]
    [Export(typeof(IWpfTextViewCreationListener))]
    [TextViewRole(PredefinedTextViewRoles.PrimaryDocument)]
    public class TextViewCreationListener : IWpfTextViewCreationListener
    {
        [Export(typeof(AdornmentLayerDefinition))]
        [Name("test")]
        [Order(After = PredefinedAdornmentLayers.Text, Before = PredefinedAdornmentLayers.Caret)]
        public static AdornmentLayerDefinition UIAdornmentLayer { get; set; }

        private IAdornmentLayer _layer;
        private ITrackingSpan _trackingSpan;
        private IWpfTextView _textView;

        public void TextViewCreated(IWpfTextView textView)
        {
            _textView = textView;
            _layer = _textView.GetAdornmentLayer("test");
            textView.Caret.PositionChanged += Caret_PositionChanged;
        }

        private async void Caret_PositionChanged(object sender, CaretPositionChangedEventArgs e)
        {
            var currentSnapshot = e.TextView.TextSnapshot;
            var document = currentSnapshot.GetRelatedDocumentsWithChanges().FirstOrDefault();
            if (document == null)
                return;

            var span = await GetMethodIdentifierSpan(document, e.NewPosition.BufferPosition);
            AdornSpan(currentSnapshot, span);
        }

        private void AdornSpan(ITextSnapshot snapshot, Span span)
        {
            if (_trackingSpan != null)
            {
                var oldSnapshot = this._trackingSpan.GetSpan(snapshot);
                //Visual Studio is already tracking this span
                if (oldSnapshot.OverlapsWith(span))
                {
                    return;
                }
                // Any actions we need to take when user navigates to a new method go here:
                ClearUIAdornments(); // Clears launch/stop adornment from the previous method
            }

            var line = snapshot.GetLineFromPosition(span.Start);
            Geometry g = _textView.TextViewLines.GetMarkerGeometry(line.Extent);

            //The method's identifier is offscreen so we can draw the adornment
            if (g == null)
            {
                return;
            }

            //Now we create our adornment
            _trackingSpan = snapshot.CreateTrackingSpan(span, SpanTrackingMode.EdgeInclusive);
            //orangeAdornment = new LaunchOrangeAdornment(_textView, _trackingSpan, _filePath);
            //Canvas.SetLeft(orangeAdornment, g.Bounds.Left);
            //Canvas.SetTop(orangeAdornment, g.Bounds.Top);
            //_layer.AddAdornment(AdornmentPositioningBehavior.TextRelative, line.Extent, null, orangeAdornment, null);
        }
        
        /// <summary>
        /// Physically removes the launch button adornments from the screen
        /// Used when caret changes method.
        /// </summary>
        public void ClearUIAdornments()
        {
            _trackingSpan = null;
            _layer.RemoveAllAdornments();
        }

        private async Task<Span> GetMethodIdentifierSpan(Document document, int position)
        {
            var root = await document.GetSyntaxRootAsync();

            var containingMethod = root.DescendantNodes().OfType<BaseMethodDeclarationSyntax>().Where(n => n.Span.Contains(position)).FirstOrDefault();
            if (containingMethod == null)
                return default(Span);

            if (containingMethod.Modifiers.Any(SyntaxKind.AbstractKeyword)
                || containingMethod.Modifiers.Any(SyntaxKind.ExternKeyword))
            {
                return default(Span);
            }

            TextSpan identifierSpan = default(TextSpan);
            if (containingMethod is MethodDeclarationSyntax)
            {
                identifierSpan = ((MethodDeclarationSyntax)containingMethod).Identifier.Span;
            }
            else if (containingMethod is ConstructorDeclarationSyntax)
            {
                identifierSpan = ((ConstructorDeclarationSyntax)containingMethod).Identifier.Span;
            }
            else if (containingMethod is DestructorDeclarationSyntax)
            {
                identifierSpan = ((DestructorDeclarationSyntax)containingMethod).Identifier.Span;
            }
            else if (containingMethod is ConversionOperatorDeclarationSyntax)
            {
                identifierSpan = ((ConversionOperatorDeclarationSyntax)containingMethod).Type.Span;
            }
            else if (containingMethod is OperatorDeclarationSyntax)
            {
                identifierSpan = ((OperatorDeclarationSyntax)containingMethod).OperatorToken.Span;
            }
            //If there's no identifier on the method (perhaps they erased it) return a default span.
            if (identifierSpan == default(TextSpan))
                return default(Span);
            var span = new Span(identifierSpan.Start, identifierSpan.Length);
            return span;
        }
    }
}
