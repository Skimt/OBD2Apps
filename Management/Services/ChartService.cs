using ChartJs.Blazor.ChartJS.Common;
using ChartJs.Blazor.ChartJS.Common.Axes;
using ChartJs.Blazor.ChartJS.Common.Axes.Ticks;
using ChartJs.Blazor.ChartJS.Common.Enums;
using ChartJs.Blazor.ChartJS.Common.Handlers;
using ChartJs.Blazor.ChartJS.Common.Properties;
using ChartJs.Blazor.ChartJS.Common.Time;
using ChartJs.Blazor.ChartJS.LineChart;
using ChartJs.Blazor.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace Management.Services
{
	public class ChartService
	{
		
		/// <summary>
		/// Returns a dataset that has been preconfigured for a linechart (timechart) in Blazor Chart.JS. 
		/// </summary>
		public Task<LineDataset<TimeTuple<float>>> GetLineData(string label, Color color)
		{

			//var lineColor = ColorUtil.RandomColorString();
			var lineColor = ColorUtil.FromDrawingColor(color);

			return Task.FromResult(new LineDataset<TimeTuple<float>>
			{
				BackgroundColor = lineColor,
				BorderColor = lineColor,
				Label = label,
				Fill = false,
				BorderWidth = 2,
				PointRadius = 0,
				PointBorderWidth = 1,
				SteppedLine = SteppedLine.False
			});
		}

		/// <summary>
		/// Returns a preconfiguration for a linechart (timechart) in Blazor Chart.JS.
		/// It was written as a service to prevent the front-end code from bloating. 
		/// </summary>
		public Task<LineConfig> GetLineConfig()
		{

			return Task.FromResult(new LineConfig
			{
				Options = new LineOptions
				{
					MaintainAspectRatio = false,
					Responsive = true,
					Title = new OptionsTitle
					{
						Display = false
					},
					Legend = new Legend
					{
						Position = Position.Bottom,
						Labels = new LegendLabelConfiguration
						{
							UsePointStyle = true
						}
					},
					Tooltips = new Tooltips
					{
						Mode = InteractionMode.Nearest,
						Intersect = false
					},
					Scales = new Scales
					{
						xAxes = new List<CartesianAxis>
						{
							new TimeAxis
							{
								Distribution = TimeDistribution.Linear,
								Ticks = new TimeTicks
								{
									Source = TickSource.Data
								},
								Time = new TimeOptions
								{
									Unit = TimeMeasurement.Millisecond,
									Round = TimeMeasurement.Millisecond,
									TooltipFormat = "DD.MM.YYYY HH:mm:ss:SSS",
									DisplayFormats = TimeDisplayFormats.DE_CH
								}
							}
						}
					},
					Hover = new LineOptionsHover
					{
						Intersect = true,
						Mode = InteractionMode.Y
					}
				}
			});

		}

		/// <summary>
		/// Returns a preconfiguration for a linechart (timechart) in Blazor Chart.JS.
		/// It was written as a service to prevent the front-end code from bloating. 
		/// </summary>
		public Task<LineConfig> GetMultiLineConfig()
		{

			return Task.FromResult(new LineConfig
			{
				Options = new LineOptions
				{
					MaintainAspectRatio = false,
					Responsive = true,
					Title = new OptionsTitle
					{
						Display = false
					},
					Legend = new Legend
					{
						Position = Position.Bottom,
						Labels = new LegendLabelConfiguration
						{
							UsePointStyle = true
						}
					},
					Tooltips = new Tooltips
					{
						Mode = InteractionMode.Nearest,
						Intersect = false
					},
					Scales = new Scales
					{
						xAxes = new List<CartesianAxis>
						{
							new TimeAxis
							{
								Distribution = TimeDistribution.Linear,
								Ticks = new TimeTicks
								{
									Source = TickSource.Data
								},
								Time = new TimeOptions
								{
									Unit = TimeMeasurement.Millisecond,
									Round = TimeMeasurement.Millisecond,
									TooltipFormat = "DD.MM.YYYY HH:mm:ss:SSS",
									DisplayFormats = TimeDisplayFormats.DE_CH
								}
							}
						}
					},
					Hover = new LineOptionsHover
					{
						Intersect = true,
						Mode = InteractionMode.Y
					}
				}
			});

		}

	}
}
