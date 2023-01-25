/*
	This file is part of TweakScalerModuleWaterfallFXIntegrator, a component of TweakScaleCompanion_Frameworks
	© 2021-2023 LisiasT : http://lisias.net <support@lisias.net>

	TweakScaleCompanion_Frameworks is double licensed, as follows:
		* SKL 1.0 : https://ksp.lisias.net/SKL-1_0.txt
		* GPL 2.0 : https://www.gnu.org/licenses/gpl-2.0.txt

	And you are allowed to choose the License that better suit your needs.

	TweakScaleCompanion_Frameworks is distributed in the hope that it will be useful,
	but WITHOUT ANY WARRANTY; without even the implied warranty of
	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.

	You should have received a copy of the SKL Standard License 1.0
	along with TweakScaleCompanion_Frameworks. If not, see <https://ksp.lisias.net/SKL-1_0.txt>.

	You should have received a copy of the GNU General Public License 2.0
	along with TweakScaleCompanion_Frameworks. If not, see <https://www.gnu.org/licenses/>.

*/
using System.Collections.Generic;
using TweakScale;
using ModuleWaterfallFX = global::Waterfall.ModuleWaterfallFX;

namespace TweakScaleCompanion.Frameworks.Waterfall.Integrator
{
	public class Implementation : IRescalable, Notifier
	{
		private readonly Listener listener;
		private readonly Part part;
		private readonly TweakScale.TweakScale tweakscale;
		private readonly ModuleWaterfallFX[] targetPartModules;
		private readonly List<Data> originalFx = new List<Data>();

		public Implementation(Part part, Listener listener)
		{
			this.listener = listener;
			this.part = part;

			this.tweakscale = this.part.Modules.GetModule<TweakScale.TweakScale>();
			if (null == this.tweakscale) throw new System.NullReferenceException("TweakScale not found!");

			List<ModuleWaterfallFX> l = this.part.Modules.GetModules<ModuleWaterfallFX>();
			if (null == l)  throw new System.NullReferenceException("ModuleWaterfallFX not found!");
			this.targetPartModules = l.ToArray();
		}

		bool Notifier.IsEnabled()
		{
			return this.IsEnabled();
		}

		void Notifier.Init()
		{
			Log.dbg("InitInternalData {0}", this.InstanceID);

			this.originalFx.Clear();
			foreach(ModuleWaterfallFX m in this.targetPartModules)
				foreach (global::Waterfall.WaterfallEffect fx in m.FX)
					this.originalFx.Add(new Data(fx));
		}

		void Notifier.Update()
		{
			ScalingFactor factor = this.tweakscale.ScalingFactor;
			Log.dbg("UpdateTarget {0} by {1}", this.InstanceID, factor.absolute.linear);
			if (null == this.targetPartModules) return;

			foreach (Data data in this.originalFx)
				this.scale(data, factor);
		}

		#region Part Events Handlers

		void IRescalable.OnRescale(ScalingFactor factor)
		{
			Log.dbg("OnRescale {0} to {1}", this.listener.GetName(), this.InstanceID, factor.absolute.linear);

			if (this.IsEnabled())
				this.listener.NotifyRestoreNeeded();
		}

		#endregion

		private bool IsEnabled()
		{
			bool enabled = false;
			foreach (ModuleWaterfallFX m in this.targetPartModules)
				enabled |= m.enabled;
			return enabled;
		}

		private void scale(Data data, ScalingFactor factor)
		{
			data.fx.ApplyTemplateOffsets(data.position, data.fx.TemplateRotationOffset, data.meshScale * factor.absolute.linear);
		}

		private static KSPe.Util.Log.Logger Log = KSPe.Util.Log.Logger.CreateForType<TweakScalerWaterfallFX>("TweakScaleCompanion_Frameworks", "TweakScalerWaterfallFX");
		private string InstanceID => string.Format("{0}:{1:X}", this.part.name, (null == this.part ? 0 : this.part.GetInstanceID()));
	}
}
