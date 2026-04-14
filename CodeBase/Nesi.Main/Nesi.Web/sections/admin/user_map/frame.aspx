<%@ Page Language="C#" MasterPageFile="~/IntraDefault.master" AutoEventWireup="true" Inherits="map_frame" Title="Employee Map Interface" Codebehind="frame.aspx.cs" %>

<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="cphMasterMenu" runat="Server">
    <div id="divMenu" runat="server">
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMasterLeft" runat="Server">
    <div id="divSide" runat="server">
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMasterSubMenu" runat="Server">
    
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMasterBody" runat="Server">




    <link href="/css/maps/css.css" rel="Stylesheet" type="text/css" />
    <link href="/css/maps/showLoading.css" rel="stylesheet" media="screen" />
    <link rel="stylesheet" href="//code.jquery.com/ui/1.11.4/themes/smoothness/jquery-ui.css" />
    <!--script type="text/javascript" src="//ajax.googleapis.com/ajax/libs/jquery/2.1.3/jquery.min.js"></!--script >
    <script type="text/javascript" src="//code.jquery.com/ui/1.11.4/jquery-ui.js"></script -->
    <script type="text/javascript" src="//maps.googleapis.com/maps/api/js?key=AIzaSyBBCtFiensGTxpITOFEbzY8xRXPQYMkDEY">
    </script>
    <script type="text/javascript" src="/js/maps/markerclusterer.js"></script>
    <script type="text/javascript">
        var markers = [];
        var markerClusterer = null;
        $(document).ready(function () {
            jQuery.browser = {};
            (function () {
                jQuery.browser.msie = false;
                jQuery.browser.version = 0;
                if (navigator.userAgent.match(/MSIE ([0-9]+)\./)) {
                    jQuery.browser.msie = true;
                    jQuery.browser.version = RegExp.$1;
                }
            })();
            //            MapAutoHeight();




            //TODO: JA should Load the map for your BU area
            LoadMap(43.6563596, -79.3806738);
        })


        function LoadMap(lat, lot) {


            navigator.geolocation.getCurrentPosition(
                    function (position) {
                        latitude = position.coords.latitude;
                        longitude = position.coords.longitude;

                        var map = new google.maps.Map(document.getElementById('map_canvas'),
                            {
                                zoom: 10,
                                center: new google.maps.LatLng(latitude, longitude),
                                mapTypeId: google.maps.MapTypeId.ROADMAP
                            });
                        google.maps.event.addListener(map, 'click', function (event) {
                            var lat = event.latLng.lat();
                            var lng = event.latLng.lng();
                            // populate yor box/field with lat, lng
                            alert("Lat=" + lat + "; Lng=" + lng);
                            whoishere_callbackPan.PerformCallback(lat + "&" + lng);
                        });
                    }, function (err) {
                        console.log("Geolocation Not allowed");
                        var map = new google.maps.Map(document.getElementById('map_canvas'),
                    {
                        zoom: 10,
                        center: new google.maps.LatLng(lat, lot),
                        mapTypeId: google.maps.MapTypeId.ROADMAP
                    });

                    });

        }

        function DisMarker() {

            $("#loader").html("LOADING!");
            //$(this).html("test");
            var pho = new Array();
            $("input[name='chkPh']:checkbox:checked").each(function () {
                pho.push($(this).attr("id"));
            })
            if (pho.length <= 0) {
                LoadMap(43.6563596, -79.3806738);
                ClearSpanColor();

                $("#loader").html("");
                return;
            }


            ClearSpanColor();
            ClearMarker();
            showMap(pho.toString());

        }

       

        function MakeMarker(result) {
            var obj = eval(result);
            var pho = obj[0].Pho;
            var col = (obj[0].Col);
            var id = obj[0].id;

            $(".chk_box").css('background-color', "transparent");
            



            for (var j = 0; j < obj.length; j++) {
                $("#chk_" + obj[j].id).css('background-color', "#" + obj[j].Col);
            }


            var map = new google.maps.Map(document.getElementById('map_canvas'),
            {
                zoom: 10,
                center: new google.maps.LatLng(43.656359, -79.3806738),
                mapTypeId: google.maps.MapTypeId.ROADMAP
            });
        
           



                if (col != "" && col != null) {
                markers = GetMarkers(markers, obj, map);
                MakeMarkerByClusterer(map, markers, obj);
            } 
        }
        
        function MakeMarkerByClusterer(map, markers, obj) {

            var styles = [[{
                url: '/css/maps/people35.png',
                height: 35,
                width: 35,
                anchor: [16, 0],
                textColor: '#ff00ff',
                textSize: 10
            }, {
                url: '/css/maps/people45.png',
                height: 45,
                width: 45,
                anchor: [24, 0],
                textColor: '#ff0000',
                textSize: 11
            }, {
                url: '/css/maps/people55.png',
                height: 55,
                width: 55,
                anchor: [32, 0],
                textColor: '#ffffff',
                textSize: 12
            }], [{
                url: '/css/maps/conv30.png',
                height: 27,
                width: 30,
                anchor: [3, 0],
                textColor: '#ff00ff',
                textSize: 10
            }, {
                url: '/css/maps/conv40.png',
                height: 36,
                width: 40,
                anchor: [6, 0],
                textColor: '#ff0000',
                textSize: 11
            }, {
                url: '/css/maps/conv50.png',
                width: 50,
                height: 45,
                anchor: [8, 0],
                textSize: 12
            }, {
                url: '/css/maps/m1.png',
                width: 56,
                height: 55,
                anchor: [4, 0],
                textSize: 12
            }, {
                url: '/css/maps/m2.png',
                width: 56,
                height: 55,
                anchor: [6, 0],
                textSize: 12
            }, {
                url: '/css/maps/m3.png',
                width: 56,
                height: 55,
                anchor: [8, 0],
                textSize: 12
            }]];
            markerClusterer = new MarkerClusterer(map, markers, {
                maxZoom: 13,
                gridSize: 50,
                minimumClusterSize: 5,
                styles: styles[6]
            });
        }

        var infowindow = new google.maps.InfoWindow({});
        function GetMarkers(markers, obj, map) {

            var bounds = new google.maps.LatLngBounds();
            $.each(obj, function (i, item) {
                var pho = obj[i].Pho;
                var col = obj[i].Col;
                var name = obj[i].Name;
                var id = obj[i].id;
                SetCheckColor(pho, col);
                var locations = obj[i].Locations;
                $.each(locations, function (no, itemLo) {
                    var lat = locations[no].lat;
                    var lng = locations[no].lng;
                    var num = locations[no].num;
                    var title = pho + " " + locations[no].time + "\n" + "lat:" + lat + " lng:" + lng;
                    var forecolor = getContrastYIQ(col);
                    var icon = "//chart.apis.google.com/chart?chst=d_map_pin_letter&chld=" + num + "|" + col + "|" + forecolor;
                    icon = "//thydzik.com/thydzikGoogleMap/markerlink.php?text=" + num + "&color=" + col
                    var latLng = new google.maps.LatLng(lat, lng);
                    bounds.extend(latLng);
                    var markerImage = new google.maps.MarkerImage(icon, null, null, null, null);
                    /*var shape = {
                        coord: [20, 20, 20, 20],
                        type: 'rect'
                    };*/
                    var marker = new google.maps.Marker(
                     {
                         position: latLng,
                         title: title,
                         draggable: false,
                         map: map,
                         icon: icon,
                         //shape: shape,
                         optimized: true
                     });
                    //Click Event
                    google.maps.event.addListener(marker, 'click', function (e) {
                        //infowindow.setContent("<div style='height:90px;width:400px;'><img src='/_tools/member_photo/index.aspx?member_id=" + id + "' width='auto' height='85' style='border:solid 1px #000; float:left;margin-right:15px;'/><div style='float:left;'><b>Employee: </b>" + name + "<br/><b>Date/Time: </b>" + locations[no].time + "<br/><b>Latitude: </b>" + e.latLng.lat() + "<br/><b>Longitude: </b>" + e.latLng.lng() + "</div></div>");
                        infowindow.setContent("<div style='height:90px;width:400px;'><img src='/_tools/member_photo/index.aspx?member_id=" + id + "' width='auto' height='85' style='border:solid 1px #000; float:left;margin-right:15px;'/><div style='float:left;'><b>Employee: </b>" + name + "<br/><b>Date/Time: </b>" + locations[no].time + "<br/><b>Battery: </b>" + locations[no].bat+ "<br/></div></div>");
                        infowindow.open(map, marker);
                    });
                    markers.push(marker);
                });
            });
            map.fitBounds(bounds);
            return markers;
        }

        function getContrastYIQ(hexcolor) {

            var r = parseInt(hexcolor.substr(0, 2), 16);
            var g = parseInt(hexcolor.substr(2, 2), 16);
            var b = parseInt(hexcolor.substr(4, 2), 16);
            var yiq = ((r * 299) + (g * 587) + (b * 114)) / 1000;
            return (yiq >= 128) ? '000000' : 'ffffff';
        }

        function MakeMarkerIcon(num, rgb) {

            var icon = "//chart.apis.google.com/chart?chst=d_map_pin_letter&chld=" + num + "|" + rgb + "|000000";
            icon = "//thydzik.com/thydzikGoogleMap/markerlink.php?text=" + num + "&color=" + rgb;
            return icon;
        }

        function ClearMarker() {

            if (markers) {
                for (i in markers) {
                    markers[i].setMap(null);
                }
                markers.length = 0;
            }
        }

        function SetCheckColor(obj, color) {
            var forecolor = getContrastYIQ(color);
            $("#span" + obj).css({ "background-color": "#" + color, "color": "#" + forecolor });
        }

        function ClearSpanColor() {
            $("span").each(function () { $(this).css({ "background-color": "transparent", "color": "#000" }); });
        }
        function SetCheck(phone) {
            var chk = document.getElementById(phone);
            chk.checked = !chk.checked;
        }

        function MapAutoHeight() {
            var wh = $(window).height();
            var height = wh - document.getElementById("header").offsetHeight;
            $("#mainbody").height(height);
        }

        function GetLocation(map) {
            google.maps.event.addListener(map, "rightclick", function (event) {
                var lat = event.latLng.lat();
                var lng = event.latLng.lng();
                txtLocation.value = "Lat=" + lat + "; Lng=" + lng
            });
        }



    </script>

    <div id="header">
        <table style="padding-left: 15px; width: 650px">
            <tr>
                <td style="width: 20px"><b>From:</b></td>
                <td style="width: 120px">
                    <dx:ASPxDateEdit ID="dateFrom" ClientInstanceName="dateFrom" runat="server">
                        <TimeSectionProperties Visible="True">
                        </TimeSectionProperties>
                        <ClientSideEvents ValueChanged="function(s, e) {
	chk_callBackPan.PerformCallback();
}" />
                    </dx:ASPxDateEdit>
                </td>
                <td style="width: 20px"><b>To:</b></td>
                <td style="width: 120px">
                    <dx:ASPxDateEdit ID="dateTo" runat="server" ClientInstanceName="dateTo" >
                        <TimeSectionProperties Visible="True">
                        </TimeSectionProperties>
                        <ClientSideEvents ValueChanged="function(s, e) {
	chk_callBackPan.PerformCallback();
}" />
                    </dx:ASPxDateEdit>
                </td>
                <td style="width: 120px">
                    <dx:ASPxCheckBox ID="chkBox" runat="server" Text="Last Known Location" CheckState="Unchecked">
                        <ClientSideEvents ValueChanged="function(s, e) {
	chk_callBackPan.PerformCallback();
}" />
                    </dx:ASPxCheckBox>
                </td>
                
                <td style="width: 120px" >
                    <dx:ASPxCheckBox ID="chk_whoNear" runat="server" Text="Whos Near" CheckState="Unchecked" Visible="false">
                    </dx:ASPxCheckBox>
                    <dx:ASPxTextBox ID="near_distance" runat="server" Width="30px" Text="5" Visible="false" >
                    </dx:ASPxTextBox>
                </td>
            </tr>
        </table>
    </div>


    <table cellpadding="5" cellspacing="0" width="100%">
        <tr>
            <td id="sidebar" valign="top">
                <dx:ASPxNavBar ID="userSelector" runat="server" AutoCollapse="False" Theme="NETheme01">
                    <GroupHeaderStyle Font-Bold="True" ForeColor="#242424">
                    </GroupHeaderStyle>
                    <GroupContentStyle BackColor="#F9F9F9" ForeColor="#3E3E3E">
                    </GroupContentStyle>
                    <ItemTemplate>
                        <table style="width: 100%;" id ='<%# "chk_" + Eval("Name") %>' class="chk_box">
                            <tr>
                                <td style="overflow: hidden">
                                    <dx:ASPxCheckBox runat="server" ID="chk" OnInit="chk_Init" ClientIDMode="AutoID"  Text='<%# Eval("Text") %>' Theme="NETheme01" Wrap="True">
                                    </dx:ASPxCheckBox>
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                </dx:ASPxNavBar>
            </td>
            <td id="mainbody" valign="top">
                <div id="map_canvas" style="height: 750px;"></div>
            </td>
        </tr>
    </table>

    

    <dx:ASPxCallbackPanel ID="chk_callBackPan" ClientInstanceName="chk_callBackPan" runat="server" Width="200px" OnCallback="chk_callBackPan_Callback">
        <ClientSideEvents EndCallback="function(s, e) {
            ClearMarker()
	        MakeMarker(chk_callBackPan.cpJson);
        }" />

        <PanelCollection>
<dx:PanelContent runat="server"></dx:PanelContent>
</PanelCollection>
    </dx:ASPxCallbackPanel>
        <dx:ASPxCallbackPanel ID="whoishere_callbackPan" ClientInstanceName="whoishere_callbackPan" runat="server" Width="200px" OnCallback="whoishere_callBackPan_Callback">
        <ClientSideEvents EndCallback="function(s, e) {
            ClearMarker()
	        MakeMarker(whoishere_callbackPan.cpJson);
        }" />

        <PanelCollection>
<dx:PanelContent runat="server"></dx:PanelContent>
</PanelCollection>
    </dx:ASPxCallbackPanel>
</asp:Content>

