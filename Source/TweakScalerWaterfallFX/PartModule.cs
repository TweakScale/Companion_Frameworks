/*
	This file is part of TweakScalerModuleWaterfallFX, a component of TweakScaleCompanion_Visuals
	© 2021 LisiasT : http://lisias.net <support@lisias.net>

	TweakScaleCompanion_Visuals is double licensed, as follows:

	* SKL 1.0 : https://ksp.lisias.net/SKL-1_0.txt
	* GPL 2.0 : https://www.gnu.org/licenses/gpl-2.0.txt

	And you are allowed to choose the License that better suit your needs.

	TweakScaleCompanion_Visuals is distributed in the hope that it will be useful,
	but WITHOUT ANY WARRANTY; without even the implied warranty of
	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.

	You should have received a copy of the SKL Standard License 1.0
	along with TweakScaleCompanion_Visuals. If not, see <https://ksp.lisias.net/SKL-1_0.txt>.

	You should have received a copy of the GNU General Public License 2.0
	along with TweakScaleCompanion_Visuals. If not, see <https://www.gnu.org/licenses/>.

*/
using System;
using System.Collections.Generic;
using TweakScale;
using TweakScale.Annotations;
using UnityEngine;
using ModuleWaterfallFX = global::Waterfall.ModuleWaterfallFX;

namespace TweakScaleCompanion.Visuals.Waterfall
{
	public class TweakScalerWaterfallFX : PartModule, IRescalable
	{
		private class Data
		{
			public readonly global::Waterfall.WaterfallEffect fx;
			public readonly Vector3 meshScale;
			public readonly Vector3 position;

			public Data(global::Waterfall.WaterfallEffect fx, TweakScalerWaterfallFX myself)
			{
				this.fx = fx;
				this.meshScale = fx.TemplateScaleOffset;
				this.position = fx.TemplatePositionOffset;
			}
		}

		#region KSP UI

		// No GUI at the moment

		#endregion


		#region Part Module Fields

		/// <summary>
		/// Whether the Helper was deativated by some reason (usually the Sanity Checks)
		/// </summary>
		[KSPField(isPersistant = false)]
		public bool isActive = false;

		#endregion

		private TweakScale.TweakScale tweakscale;
		private ModuleWaterfallFX[] targetPartModules;
		private readonly List<Data> originalFx = new List<Data>();

		private bool IsRestoreNeeded = false;
		private bool IsInitNeeded = true;

		#region KSP Life Cycle

		public override void OnAwake()
		{
			Log.dbg("OnAwake {0}", this.InstanceID);
			base.OnAwake();
		}

		public override void OnStart(StartState state)
		{
			Log.dbg("OnStart {0} {1}", this.name, this.InstanceID, state);
			base.OnStart(state);

			this.IsInitNeeded = true;
			this.IsRestoreNeeded = true;
		}

		public override void OnCopy(PartModule fromModule)
		{
			Log.dbg("OnCopy {0} from {1:X}", this.InstanceID, fromModule.part.GetInstanceID());
			base.OnCopy(fromModule);

			// Needed because I can't intialize this on OnAwake as this module can be awaken before ModuleWaterfallFX,
			// and OnRescale can be fired before OnLoad.
			if (null == this.targetPartModules) this.IsInitNeeded = true;

			this.IsRestoreNeeded = true;
			this.enabled = true; // To allow the "FSM" on Update to run!
		}

		public override void OnLoad(ConfigNode node)
		{
			Log.dbg("OnLoad {0} {1}", this.InstanceID, null == node ? "prefab" : node.name);
			base.OnLoad(node);
			if (null == node) return;   // Load from Prefab - not interesting.

			// Needed because I can't intialize this on OnAwake as this module can be awaken before ModuleWaterfallFX,
			// and OnRescale can be fired before OnLoad.
			if (null == this.targetPartModules)
			{
				this.IsInitNeeded = true;
			}
			this.IsRestoreNeeded = true;
		}

		public override void OnSave(ConfigNode node)
		{
			Log.dbg("OnSave {0} {1}", this.InstanceID, null != node);
			base.OnSave(node);
		}

		#endregion


		#region Unity Life Cycle
		 
		[UsedImplicitly]
		private void Update()
		{
			if (this.IsInitNeeded)
			{
				// Needed because I can't intialize this on OnAwake as this module can be awaken before ModuleWaterfallFX or TweakScale,
				// and OnRescale can be fired before OnLoad.
				// Note: On KSP 1.12, the this.part.Modules are not completely filed as OnStart is called, so now we need
				// to do it here.
				// See https://forum.kerbalspaceprogram.com/index.php?/topic/192216-tweakscale-companion-program-2021-0201/&do=findComment&comment=3995406
				if (this.IsInitNeeded = this.InitModule())
					if (HighLogic.LoadedSceneIsFlight) // For some reason I could not understand, I can't initialise Waterfall from the Editor.
						this.InitInternalData();
			}

			if (this.IsRestoreNeeded)
			{
				this.UpdateTarget(this.tweakscale.ScalingFactor);
				this.IsRestoreNeeded = false;
			}
		}

		[UsedImplicitly]
		private void OnDestroy()
		{
			Log.dbg("OnDestroy {0}", this.InstanceID);

			// The object can be destroyed before the full initialization cycle while KSP is booting, so we need to check first.
			if (null == this.targetPartModules) return;
		}

		#endregion


		#region Part Events Handlers

		void IRescalable.OnRescale(ScalingFactor factor)
		{
			Log.dbg("OnRescale {0} to {1}", this.name, this.InstanceID, factor.absolute.linear);

			this.IsRestoreNeeded = true;
		}

		#endregion

		private bool InitModule()
		{
			this.tweakscale = this.part.Modules.GetModule<TweakScale.TweakScale>();
			if (null == this.tweakscale) return false;

			List<ModuleWaterfallFX> l = this.part.Modules.GetModules<ModuleWaterfallFX>();
			if (null == l) return false;
			this.targetPartModules = l.ToArray();

			this.enabled = false;
			foreach (ModuleWaterfallFX m in this.targetPartModules)
				this.enabled |= m.enabled;
			return true;
		}

		private void InitInternalData()
		{
			Log.dbg("InitInternalData {0}", this.InstanceID);

			this.originalFx.Clear();
			foreach(ModuleWaterfallFX m in this.targetPartModules)
				foreach (global::Waterfall.WaterfallEffect fx in m.FX)
					this.originalFx.Add(new Data(fx, this));
		}

		private void UpdateTarget(ScalingFactor factor)
		{
			Log.dbg("UpdateTarget {0} by {1}", this.InstanceID, factor.absolute.linear);
			if (null == this.targetPartModules) return;

			foreach (Data data in this.originalFx)
				this.scale(data, factor);
		}

		private void scale(Data data, ScalingFactor factor)
		{
			data.fx.ApplyTemplateOffsets(data.position, data.fx.TemplateRotationOffset, data.meshScale * factor.absolute.linear);
		}

		private static KSPe.Util.Log.Logger Log = KSPe.Util.Log.Logger.CreateForType<TweakScalerWaterfallFX>("TweakScaleCompanion_Visuals", "TweakScalerWaterfallFX");

		static TweakScalerWaterfallFX()
		{
			Log.level =
#if DEBUG
				KSPe.Util.Log.Level.TRACE
#else
				KSPe.Util.Log.Level.INFO
#endif
				;
		}
		private string InstanceID => string.Format("{0}:{1:X}", this.name, (null == this.part ? 0 : this.part.GetInstanceID()));
	}

	public class Scaler : TweakScale.IRescalable<TweakScalerWaterfallFX>
	{
		private readonly TweakScale.IRescalable pm;

		public Scaler(TweakScalerWaterfallFX pm)
		{
			this.pm = pm;
		}

		public void OnRescale(ScalingFactor factor)
		{
			this.pm.OnRescale(factor);
		}
	}
}
