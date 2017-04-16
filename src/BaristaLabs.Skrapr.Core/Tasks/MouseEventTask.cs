﻿namespace BaristaLabs.Skrapr.Tasks
{
    using BaristaLabs.Skrapr.Extensions;
    using System;
    using System.Threading.Tasks;
    using Input = ChromeDevTools.Input;
    using Css = ChromeDevTools.CSS;
    using Dom = ChromeDevTools.DOM;
    using Page = ChromeDevTools.Page;
    using System.Linq;

    public class MouseEventTask : ITask
    {
        public MouseEventTask()
        {
            Type = "mouseMoved";
        }

        public string Name
        {
            get { return "MouseEvent"; }
        }

        public string Selector
        {
            get;
            set;
        }

        /// <summary>
        /// mouseMoved, mousePressed, mouseReleased
        /// </summary>
        public string Type
        {
            get;
            set;
        }

        /// <summary>
        /// none, left, middle, right
        /// </summary>
        public string Button
        {
            get;
            set;
        }

        public async Task PerformTask(SkraprContext context)
        {
            var documentNode = await context.Session.DOM.GetDocument(1);

            var nodeIds = await context.Session.DOM.QuerySelectorAll(new Dom.QuerySelectorAllCommand
            {
                NodeId = documentNode.NodeId,
                Selector = ".MENS"
            });

            var nodeId = nodeIds.NodeIds.First();
            if (nodeId < 1)
                return;

            var highlightObject = await context.Session.DOM.GetHighlightObjectForTest(nodeId);
            var contentPath = highlightObject.Paths.FirstOrDefault(p => p.Name == "content");
            var contentPathPoints = contentPath.GetQuad();

            var scaleFactor = await context.DevTools.GetPageScaleFactor();
            var xScaleFactor = scaleFactor.Item1;
            var yScaleFactor = scaleFactor.Item2;

            var targetRect = new Dom.Rect
            {
                X = contentPathPoints[0] / xScaleFactor,
                Y = contentPathPoints[1] / yScaleFactor,
                Width = highlightObject.ElementInfo.NodeWidth / xScaleFactor,
                Height = highlightObject.ElementInfo.NodeHeight / yScaleFactor //2.2
            };

            await context.Session.DOM.HighlightRect(new Dom.HighlightRectCommand
            {
                X = (long)(targetRect.X),
                Y = (long)(targetRect.Y),
                Width = (long)(targetRect.Width),
                Height = (long)(targetRect.Height),
                Color = new Dom.RGBA
                {
                    R = 0,
                    G = 0,
                    B = 255,
                    A = 0.7
                },
                OutlineColor = new Dom.RGBA
                {
                    R = 255,
                    G = 0,
                    B = 0,
                    A = 1
                },
            });

            //Validation
            var target = targetRect.GetRandomSpotWithinRect();
            var targetNodeId = await context.Session.DOM.GetNodeForLocation(new Dom.GetNodeForLocationCommand
            {
                X = (long)target.X,
                Y = (long)target.Y
            });

            await context.Session.DOM.HighlightRect(new Dom.HighlightRectCommand
            {
                X = (long)target.X,
                Y = (long)target.Y,
                Width = 1,
                Height = 1,
                Color = new Dom.RGBA
                {
                    R = 255,
                    G = 0,
                    B = 0,
                    A = 1
                },
                OutlineColor = new Dom.RGBA
                {
                    R = 255,
                    G = 0,
                    B = 0,
                    A = 1
                },
            });

            await context.Session.DOM.HighlightNode(new Dom.HighlightNodeCommand
            {
                NodeId = targetNodeId.NodeId,
                HighlightConfig = new Dom.HighlightConfig
                {
                    ContentColor = new Dom.RGBA
                    {
                        R = 255,
                        G = 0,
                        B = 0,
                        A = 0.7
                    }
                }
            });
            //if (targetNodeId.NodeId != nodeId)
            //    throw new InvalidOperationException("A hole was torn in the universe.");

            var response = await context.Session.SendCommand<Input.DispatchMouseEventCommand, Input.DispatchMouseEventCommandResponse>(new Input.DispatchMouseEventCommand
            {
                Button = "left",
                Type = "mousePressed",
                ClickCount = 1,
                Modifiers = 0,
                X = (long)target.X,
                Y = (long)target.Y,
                Timestamp = DateTimeOffset.Now.ToUniversalTime().ToUnixTimeSeconds()
            });

            response = await context.Session.SendCommand<Input.DispatchMouseEventCommand, Input.DispatchMouseEventCommandResponse>(new Input.DispatchMouseEventCommand
            {
                Button = "left",
                Type = "mouseReleased",
                ClickCount = 1,
                Modifiers = 0,
                X = (long)target.X,
                Y = (long)target.Y,
                Timestamp = DateTimeOffset.Now.ToUniversalTime().ToUnixTimeSeconds()
            });
        }
    }
}
