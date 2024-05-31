/*
	This file is part of TweakScaleCompanion_Frameworks
	© 2021-2024 LisiasT : http://lisias.net <support@lisias.net>

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
using KSPe.UI;

namespace TweakScaleCompanion.Frameworks.GUI
{
	internal class UnmetRequirementsShowStopperAlertBox
	{
		private const string URL = "https://ksp.lisias.net/add-ons/TweakScaleCompanion/Support/Frameworks/unmet-requirements";
		private static readonly string MSG = @"Unfortunately TweakScale Companion for Frameworks is unable to proceed due unmet requiments!

You need to have {0} installed, otherwise this Companion will fail to install itself, and the Assembly will probably crash while running - what can compromise your savegame.

If you decide to proceed, do it with caution.";

		private static readonly string AMSG = @"go to TweakScale Companion Program's page, look for the dependencies for Frameworks, download and install {0} and restart KSP (it will close now)";

		internal static void Show(string failedRequirement)
		{
			KSPe.Common.Dialogs.ShowStopperAlertBox.Show(
				string.Format(MSG, failedRequirement),
				string.Format(AMSG, failedRequirement),
				() => { KSPe.Util.CkanTools.OpenURL(URL); Application.Quit(); }
			);
			Log.detail("\"Houston, we have a Problem!\" about unmet dependencies was displayed : {0}", failedRequirement);
		}
    }
}