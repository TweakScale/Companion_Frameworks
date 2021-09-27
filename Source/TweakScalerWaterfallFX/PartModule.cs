/*
	This file is part of TweakScalerModuleWaterfallFX, a component of TweakScaleCompanion_Frameworks
	© 2021 LisiasT : http://lisias.net <support@lisias.net>

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
using KSPe.Annotations;
using TweakScaleCompanion.Frameworks.Waterfall.Integrator;

namespace TweakScaleCompanion.Frameworks.Waterfall
{
	public class TweakScalerWaterfallFX : PartModule, Integrator.Listener
	{

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

		private Integrator.Notifier notifier = null;
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

			// If the Integrator's DLL was not loaded, we are dead in the water.
			if (!(this.enabled = Startup.OK_TO_GO)) return;

			this.IsInitNeeded = true;
			this.IsRestoreNeeded = true;
		}

		public override void OnCopy(PartModule fromModule)
		{
			Log.dbg("OnCopy {0} from {1:X}", this.InstanceID, fromModule.part.GetInstanceID());
			base.OnCopy(fromModule);

			// Needed because I can't intialize this on OnAwake as this module can be awaken before ModuleWaterfallFX,
			// and OnRescale can be fired before OnLoad.
			if (null == this.notifier) this.IsInitNeeded = true;

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
			if (null == this.notifier)
				this.IsInitNeeded = true;
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
						this.notifier.Init();
			}

			if (this.IsRestoreNeeded)
			{
				this.notifier.Update();
				this.IsRestoreNeeded = false;
			}
		}

		[UsedImplicitly]
		private void OnDestroy()
		{
			Log.dbg("OnDestroy {0}", this.InstanceID);

			// The object can be destroyed before the full initialization cycle while KSP is booting, so we need to check first.
			if (null == this.notifier) return;
			this.notifier = null;
		}

		#endregion

		void Integrator.Listener.NotifyRestoreNeeded()
		{
			this.IsRestoreNeeded = true;
		}

		string Listener.GetName()
		{
			return this.name;
		}

		private bool InitModule()
		{
			try
			{
				System.Type type = KSPe.Util.SystemTools.TypeFinder.FindByInterfaceName("TweakScaleCompanion.Frameworks.Waterfall.Integrator.Notifier");
				System.Reflection.ConstructorInfo ctor = type.GetConstructor(new[] { typeof(Part), typeof(Listener) });
				this.notifier = (Notifier) ctor.Invoke(new object[] { this.part, (Listener)this });
			}
			catch (System.NullReferenceException e)
			{
				Log.error(this, e);
				return false;
			}
			this.enabled = this.notifier.IsEnabled();
			return true;
		}

		private static KSPe.Util.Log.Logger Log = KSPe.Util.Log.Logger.CreateForType<TweakScalerWaterfallFX>("TweakScaleCompanion.Frameworks", "TweakScalerWaterfallFX");
		private string InstanceID => string.Format("{0}:{1:X}", this.name, (null == this.part ? 0 : this.part.GetInstanceID()));
	}

}
