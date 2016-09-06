using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExportWaypointsJson
{
  public partial class FormSettings : Form
  {
    public FormSettings()
    {
      InitializeComponent();
      txtDistance.Validating += TxtDistance_Validating;
      txtDistance.Validated += TxtDistance_Validated;
    }

    private void TxtDistance_Validated(
      object sender,
      EventArgs e )
    {
      errorProvider1.SetError( txtDistance, "" );
    }

    private void TxtDistance_Validating(
      object sender,
      CancelEventArgs e )
    {
      string s = txtDistance.Text;
      double d;
      try
      {
        d = double.Parse( s );
      }
      catch( System.FormatException )
      {
        // Cancel the event and select the text to be corrected by the user.
        e.Cancel = true;
        txtDistance.Select( 0, txtDistance.Text.Length );

        this.errorProvider1.SetError( txtDistance, "Invalid distance in metres: " + s );
      }
    }

    private void btnSave_Click(
      object sender,
      EventArgs e )
    {
      string s = txtIpAddress.Text;
      IPAddress address;
      try
      {
        address = IPAddress.Parse( s );
      }
      catch( System.FormatException )
      {
        MessageBox.Show( //this.Owner,
          "Invalid IP address: " + s,
          App.Caption,
          MessageBoxButtons.OK,
          MessageBoxIcon.Error );
        return;
      }
      Properties.Settings.Default.IpAddress = s;
      Properties.Settings.Default.DistanceInMeters = double.Parse( txtDistance.Text );

    }
  }
}
