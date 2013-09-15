<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Edge.Utilities.Salesforce.ASP.Net.Form._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Integration Page</title>
    <style type="text/css">


h3 {
    font-size: 1.2em;
}

h1, h2, h3,
h4, h5, h6 {
    color: #000;
    margin-bottom: 0;
    padding-bottom: 0;
}

    ol.round {
        list-style-type: none;
        padding-left: 0;
    }

        ol.round {
    list-style-type: none;
    padding-left: 0;
}

            ol.round li.zero,
            ol.round li.one,
            ol.round li.two,
            ol.round li.three,
            ol.round li.four,
            ol.round li.five,
            ol.round li.six,
            ol.round li.seven,
            ol.round li.eight,
            ol.round li.nine {
                background: none;
            }

        ol.round li.one {
            background: url('http://localhost:62805/Images/orderedList1.png') no-repeat;
        }

        ol.round li {
            padding-left: 10px;
            margin: 25px 0;
        }

            ol.round li {
        margin: 25px 0;
        padding-left: 45px;
    }

        h5, h6 {
    font-size: 1em;
}

    a:link, a:visited,
    a:active, a:hover {
        color: #333;
    }

    a {
    color: #333;
    outline: none;
    padding-left: 3px;
    padding-right: 3px;
    text-decoration: underline;
}

        ol.round li.two {
            background: url('http://localhost:62805/Images/orderedList2.png') no-repeat;
        }

        ol.round li.three {
            background: url('http://localhost:62805/Images/orderedList3.png') no-repeat;
        }

        </style>
</head>
<body style="height: 356px">
    <form id="form1" runat="server">
    <div>
    
    <h3>
        <asp:Image ID="edgebi_logo" runat="server" ImageUrl="https://console.edge-bi.com/assets/img/edge-logo-login.gif" />
        </h3>
        <h3 style="background-color: #00CCFF">Thank You for integrating with Edge.BI application:</h3>
        <p>Please submit the following information:</p>
    <ol class="round">
        <li class="one">
            <h5>&nbsp;</h5>
            Consumer Key
            <asp:TextBox ID="consumerKey" runat="server" Width="457px"></asp:TextBox>
&nbsp;</li>
        <li class="two">
            Secret Key&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:TextBox ID="secretKey" runat="server" Width="456px"></asp:TextBox>
&nbsp;</li>
        <li class="three">
            <h5>Integration Code : <asp:Label ID="code" runat="server" Text=" #code#"></asp:Label>
            </h5>
        </li>
        <li>
            <asp:Button ID="submit" runat="server" OnClick="submit_Click" Text="Submit" />
        </li>
    </ol>
    
    </div>
    </form>
</body>
</html>
