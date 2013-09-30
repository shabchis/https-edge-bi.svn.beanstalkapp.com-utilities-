<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Edge.Utilities.Salesforce.ASP.Net.Form._Default" %>

<!DOCTYPE html>

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
    
        <h1 style="font-size: 26px; line-height: 28px; font-weight: bold; margin: 0px; padding-bottom: 5px; color: rgb(102, 102, 102); font-family: 'Helvetica Neue', Helvetica, Arial, sans-serif; font-style: normal; font-variant: normal; letter-spacing: normal; orphans: auto; text-align: left; text-indent: 0px; text-transform: none; white-space: normal; widows: auto; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255);">
        <asp:Image ID="edgebi_logo" runat="server" ImageUrl="https://console.edge-bi.com/assets/img/edge-logo-login.gif" />
        </h1>
        <h1 style="font-size: 26px; line-height: 28px; font-weight: bold; margin: 0px; padding-bottom: 5px; color: rgb(102, 102, 102); font-family: 'Helvetica Neue', Helvetica, Arial, sans-serif; font-style: normal; font-variant: normal; letter-spacing: normal; orphans: auto; text-align: left; text-indent: 0px; text-transform: none; white-space: normal; widows: auto; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255);">&nbsp;</h1>
        <h1 style="font-size: 26px; line-height: 28px; font-weight: bold; margin: 0px; padding-bottom: 5px; color: rgb(102, 102, 102); font-family: 'Helvetica Neue', Helvetica, Arial, sans-serif; font-style: normal; font-variant: normal; letter-spacing: normal; orphans: auto; text-align: left; text-indent: 0px; text-transform: none; white-space: normal; widows: auto; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255);">Thank you for installing Edge.BI Application for Salesforce </h1>
        <p class="c0 c1" style="line-height: normal; margin: 0px; padding: 0px 0px 8px; color: rgb(102, 102, 102); font-family: 'Helvetica Neue', Helvetica, Arial, sans-serif; font-size: 13px; font-style: normal; font-variant: normal; font-weight: normal; letter-spacing: normal; orphans: auto; text-align: left; text-indent: 0px; text-transform: none; white-space: normal; widows: auto; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255);">
            <span style="color: rgb(124, 131, 124);">&nbsp;</span></p>
        <p class="c0" style="line-height: normal; margin: 0px; padding: 0px 0px 8px; color: rgb(102, 102, 102); font-family: 'Helvetica Neue', Helvetica, Arial, sans-serif; font-size: 13px; font-style: normal; font-variant: normal; font-weight: normal; letter-spacing: normal; orphans: auto; text-align: left; text-indent: 0px; text-transform: none; white-space: normal; widows: auto; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255);">
            Your Integration Code is:<span style="color: rgb(124, 131, 124); font-family: arial, helvetica, sans-serif; font-size: small;">&nbsp;&nbsp; </span> <asp:Label ID="code" runat="server" Text=" #code#"></asp:Label>
            </p>
    <ol class="round">
        <li>
        </li>
        <li></li>
    </ol>
    
    </div>
    </form>
</body>
</html>
