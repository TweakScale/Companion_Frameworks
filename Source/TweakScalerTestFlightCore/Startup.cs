/*
	This file is part of TweakScalerTestFlight, a component of TweakScaleCompanion_Frameworks
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
using UnityEngine;
using KSPe.Annotations;

namespace TweakScaleCompanion.Frameworks.TestFlightCore
{
	[KSPAddon(KSPAddon.Startup.Instantly, true)]
	internal class Startup : MonoBehaviour
	{
		private bool isTweakScale25 = false;

		[UsedImplicitly]
		private void Start()
		{
			Log.force("TweakScalerTestFlightCore Version {0} is loaded.", Version.Text);
			this.checkDependencies();
			this.loadDLLs();
		}

		private void checkDependencies()
		{
			if (KSPe.Util.SystemTools.Assembly.Exists.ByName("Scale"))
			{ 
				System.Reflection.Assembly assembly = KSPe.Util.SystemTools.Assembly.Find.ByName("Scale");
				this.isTweakScale25 = (assembly.GetName().Version.CompareTo(new System.Version(2, 4, 7)) >= 0);
			}
		}

		private void loadDLLs()
		{
			using (KSPe.Util.SystemTools.Assembly.Loader a = new KSPe.Util.SystemTools.Assembly.Loader<Startup>())
				if (this.isTweakScale25 && KSPe.Util.SystemTools.Assembly.Exists.ByName("TestFlightCore"))
					a.LoadAndStartup("TweakScalerTestFlightCore");
		}
	}
}