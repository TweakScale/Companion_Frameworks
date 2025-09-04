/*
	This file is part of TweakScalerTestFlightCoreRescalable, a component of TweakScaleCompanion_Frameworks
	© 2021-2025 LisiasT : http://lisias.net <support@lisias.net>

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
		[UsedImplicitly]
		private void Start()
		{
			Log.force("TweakScalerTestFlightCoreRescalable Version {0} is loaded.", Version.Text);
		}
	}
}
