using System;
using System.ComponentModel;
using System.Net;
using System.Windows.Forms;

namespace ExportWaypointsJson
{
  public partial class FormSettings : Form
  {
    bool _use_xml_config_settings = true;
    bool _use_json_settings = true;

    Settings _settings;

    public FormSettings()
    {
      InitializeComponent();

      if( _use_xml_config_settings )
      {
        txtIpAddress.Text = Properties.Settings.Default
          .IpAddress;

        txtDistance.Text = Properties.Settings.Default
          .DistanceInMeters.ToString( "0.##" );
      }
      if(_use_json_settings )
      {
        _settings = Settings.Load();
      }

      txtIpAddress.Validating += TxtIpAddress_Validating;
      txtIpAddress.Validated += TxtIpAddress_Validated;
      txtDistance.Validating += TxtDistance_Validating;
      txtDistance.Validated += TxtDistance_Validated;
    }

    private void TxtIpAddress_Validating( 
      object sender,
      CancelEventArgs e )
    {
      string s = txtIpAddress.Text;
      IPAddress address;
      try
      {
        address = IPAddress.Parse( s );
      }
      catch( System.FormatException )
      {
        // Cancel the event.
        e.Cancel = true;
        // Select the text to be corrected by the user.
        txtIpAddress.Select( 0, txtIpAddress.Text.Length );
        // Report error.
        this.errorProvider1.SetError( txtIpAddress,
          "Invalid IP address: " + s );
      }
    }

    private void TxtIpAddress_Validated(
      object sender,
      EventArgs e )
    {
      errorProvider1.SetError( txtIpAddress, "" );
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
        // Cancel the event.
        e.Cancel = true;
        // Select the text to be corrected by the user.
        txtDistance.Select( 0, txtDistance.Text.Length );
        // Report error.
        this.errorProvider1.SetError( txtDistance, 
          "Invalid distance in metres: " + s );
      }
    }

    private void TxtDistance_Validated(
      object sender,
      EventArgs e )
    {
      errorProvider1.SetError( txtDistance, "" );
    }

    private void btnSave_Click(
      object sender,
      EventArgs e )
    {
      if( _use_xml_config_settings )
      {
        Properties.Settings.Default.IpAddress
          = txtIpAddress.Text;

        Properties.Settings.Default.DistanceInMeters
          = double.Parse( txtDistance.Text );

        Properties.Settings.Default.Save();
      }

      if( _use_json_settings )
      {
        _settings.IpAddress = txtIpAddress.Text;
        _settings.DistanceInMetres = double.Parse( txtDistance.Text );
        _settings.Save();
      }
    }
  }
}
