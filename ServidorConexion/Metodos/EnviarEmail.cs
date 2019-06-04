using System;
using System.Net.Mail;
using System.Net;

namespace ServidorConexion.Metodos
{
    class EnviarEmail
    {
        public static bool registro(string email, string token)
        {                      
            const string asunto = "Bienvenido a DAMnificus";
            string cuerpo = header + bodyRegistro + token + bodyFinal;
            return enviarEmail(email, asunto, cuerpo);
        }
        public static bool reporte(string usuario, string email, string titulo, string reporte)
        {
            string asunto = "Reporte de " + usuario + ": " + titulo;
            string cuerpo = header + bodyReporte + reporte + bodyFinal;
            return enviarEmail(email, asunto, cuerpo);
        }
        public static bool passPerdida(string email, string token)
        {
            const string asunto = "Restaurar contraseña de DAMnificus";
            string cuerpo = header + bodyPassPerdida + token + bodyFinal;
            return enviarEmail(email, asunto, cuerpo);
        }
        private static bool enviarEmail(string email, string asunto, string cuerpo)
        {
            var fromAddress = new MailAddress("damnificusjovellanos@gmail.com", "DAMnificus Contacto"); // Cambiar esto
            const string fromPassword = "prodam$19"; // Cambiar esto
            var toAddress = new MailAddress(email);
            var smtp = new SmtpClient
            {
                UseDefaultCredentials = false,
                Host = "smtp.gmail.com",
                Port = 587,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword),
                EnableSsl = true,
                Timeout = 20000
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = asunto,
                Body = cuerpo,
                IsBodyHtml = true
            })
            {
                try
                {
                    ConsolaDebug.escribirEnConsola("INFO", "Comienzo del envio del email a traves de SSL...");
                    smtp.Send(message);
                    ConsolaDebug.escribirEnConsola("INFO", "El email se ha enviado satisfactoriamente!");
                    return true;
                }
                catch (Exception e)
                {
                    ConsolaDebug.escribirEnConsola("WARNING", "Problema al enviar el email con error:");
                    ConsolaDebug.escribirEnConsola("ERROR", e.Message);
                    return false;
                }

            }
        }

        private static string header = "<!doctype html>" +
        "<html>" +
        "  <head>" +
        "    <meta name=\"viewport\" content=\"width=device-width\" />" +
        "    <meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\" />" +
        "    <title>Simple Transactional Email</title>" +
        "    <style>" +
        "      /* -------------------------------------" +
        "          GLOBAL RESETS" +
        "      ------------------------------------- */" +
        "      " +
        "      /*All the styling goes here*/" +
        "      " +
        "      img {" +
        "        border: none;" +
        "        -ms-interpolation-mode: bicubic;" +
        "        max-width: 100%; " +
        "      }" +
        "      body {" +
        "        background-color: #f6f6f6;" +
        "        font-family: sans-serif;" +
        "        -webkit-font-smoothing: antialiased;" +
        "        font-size: 14px;" +
        "        line-height: 1.4;" +
        "        margin: 0;" +
        "        padding: 0;" +
        "        -ms-text-size-adjust: 100%;" +
        "        -webkit-text-size-adjust: 100%; " +
        "      }" +
        "      table {" +
        "        border-collapse: separate;" +
        "        mso-table-lspace: 0pt;" +
        "        mso-table-rspace: 0pt;" +
        "        width: 100%; }" +
        "        table td {" +
        "          font-family: sans-serif;" +
        "          font-size: 14px;" +
        "          vertical-align: top; " +
        "      }" +
        "      /* -------------------------------------" +
        "          BODY & CONTAINER" +
        "      ------------------------------------- */" +
        "      .body {" +
        "        background-color: #f6f6f6;" +
        "        width: 100%; " +
        "      }" +
        "      /* Set a max-width, and make it display as block so it will automatically stretch to that width, but will also shrink down on a phone or something */" +
        "      .container {" +
        "        display: block;" +
        "        margin: 0 auto !important;" +
        "        /* makes it centered */" +
        "        max-width: 580px;" +
        "        padding: 10px;" +
        "        width: 580px; " +
        "      }" +
        "      /* This should also be a block element, so that it will fill 100% of the .container */" +
        "      .content {" +
        "        box-sizing: border-box;" +
        "        display: block;" +
        "        margin: 0 auto;" +
        "        max-width: 580px;" +
        "        padding: 10px; " +
        "      }" +
        "      /* -------------------------------------" +
        "          HEADER, FOOTER, MAIN" +
        "      ------------------------------------- */" +
        "      .main {" +
        "        background: #ffffff;" +
        "        border-radius: 3px;" +
        "        width: 100%; " +
        "      }" +
        "      .wrapper {" +
        "        box-sizing: border-box;" +
        "        padding: 20px; " +
        "      }" +
        "      .content-block {" +
        "        padding-bottom: 10px;" +
        "        padding-top: 10px;" +
        "      }" +
        "      .footer {" +
        "        clear: both;" +
        "        margin-top: 10px;" +
        "        text-align: center;" +
        "        width: 100%; " +
        "      }" +
        "        .footer td," +
        "        .footer p," +
        "        .footer span," +
        "        .footer a {" +
        "          color: #999999;" +
        "          font-size: 12px;" +
        "          text-align: center; " +
        "      }" +
        "      /* -------------------------------------" +
        "          TYPOGRAPHY" +
        "      ------------------------------------- */" +
        "      h1," +
        "      h2," +
        "      h3," +
        "      h4 {" +
        "        color: #000000;" +
        "        font-family: sans-serif;" +
        "        font-weight: 400;" +
        "        line-height: 1.4;" +
        "        margin: 0;" +
        "        margin-bottom: 30px; " +
        "      }" +
        "      h1 {" +
        "        font-size: 35px;" +
        "        font-weight: 300;" +
        "        text-align: center;" +
        "        text-transform: capitalize; " +
        "      }" +
        "      p," +
        "      ul," +
        "      ol {" +
        "        font-family: sans-serif;" +
        "        font-size: 14px;" +
        "        font-weight: normal;" +
        "        margin: 0;" +
        "        margin-bottom: 15px; " +
        "      }" +
        "        p li," +
        "        ul li," +
        "        ol li {" +
        "          list-style-position: inside;" +
        "          margin-left: 5px; " +
        "      }" +
        "      a {" +
        "        color: #3498db;" +
        "        text-decoration: underline; " +
        "      }" +
        "      /* -------------------------------------" +
        "          BUTTONS" +
        "      ------------------------------------- */" +
        "      .btn {" +
        "        box-sizing: border-box;" +
        "        width: 100%; }" +
        "        .btn > tbody > tr > td {" +
        "          padding-bottom: 15px; }" +
        "        .btn table {" +
        "          width: auto; " +
        "      }" +
        "        .btn table td {" +
        "          background-color: #ffffff;" +
        "          border-radius: 5px;" +
        "          text-align: center; " +
        "      }" +
        "        .btn a {" +
        "          background-color: #ffffff;" +
        "          border: solid 1px #3498db;" +
        "          border-radius: 5px;" +
        "          box-sizing: border-box;" +
        "          color: #3498db;" +
        "          cursor: pointer;" +
        "          display: inline-block;" +
        "          font-size: 14px;" +
        "          font-weight: bold;" +
        "          margin: 0;" +
        "          padding: 12px 25px;" +
        "          text-decoration: none;" +
        "          text-transform: capitalize; " +
        "      }" +
        "      .btn-primary table td {" +
        "        background-color: #3498db; " +
        "      }" +
        "      .btn-primary a {" +
        "        background-color: #3498db;" +
        "        border-color: #3498db;" +
        "        color: #ffffff; " +
        "      }" +
        "      /* -------------------------------------" +
        "          OTHER STYLES THAT MIGHT BE USEFUL" +
        "      ------------------------------------- */" +
        "      .last {" +
        "        margin-bottom: 0; " +
        "      }" +
        "      .first {" +
        "        margin-top: 0; " +
        "      }" +
        "      .align-center {" +
        "        text-align: center; " +
        "      }" +
        "      .align-right {" +
        "        text-align: right; " +
        "      }" +
        "      .align-left {" +
        "        text-align: left; " +
        "      }" +
        "      .clear {" +
        "        clear: both; " +
        "      }" +
        "      .mt0 {" +
        "        margin-top: 0; " +
        "      }" +
        "      .mb0 {" +
        "        margin-bottom: 0; " +
        "      }" +
        "      .preheader {" +
        "        color: transparent;" +
        "        display: none;" +
        "        height: 0;" +
        "        max-height: 0;" +
        "        max-width: 0;" +
        "        opacity: 0;" +
        "        overflow: hidden;" +
        "        mso-hide: all;" +
        "        visibility: hidden;" +
        "        width: 0; " +
        "      }" +
        "      .powered-by a {" +
        "        text-decoration: none; " +
        "      }" +
        "      hr {" +
        "        border: 0;" +
        "        border-bottom: 1px solid #f6f6f6;" +
        "        margin: 20px 0; " +
        "      }" +
        "      /* -------------------------------------" +
        "          RESPONSIVE AND MOBILE FRIENDLY STYLES" +
        "      ------------------------------------- */" +
        "      @media only screen and (max-width: 620px) {" +
        "        table[class=body] h1 {" +
        "          font-size: 28px !important;" +
        "          margin-bottom: 10px !important; " +
        "        }" +
        "        table[class=body] p," +
        "        table[class=body] ul," +
        "        table[class=body] ol," +
        "        table[class=body] td," +
        "        table[class=body] span," +
        "        table[class=body] a {" +
        "          font-size: 16px !important; " +
        "        }" +
        "        table[class=body] .wrapper," +
        "        table[class=body] .article {" +
        "          padding: 10px !important; " +
        "        }" +
        "        table[class=body] .content {" +
        "          padding: 0 !important; " +
        "        }" +
        "        table[class=body] .container {" +
        "          padding: 0 !important;" +
        "          width: 100% !important; " +
        "        }" +
        "        table[class=body] .main {" +
        "          border-left-width: 0 !important;" +
        "          border-radius: 0 !important;" +
        "          border-right-width: 0 !important; " +
        "        }" +
        "        table[class=body] .btn table {" +
        "          width: 100% !important; " +
        "        }" +
        "        table[class=body] .btn a {" +
        "          width: 100% !important; " +
        "        }" +
        "        table[class=body] .img-responsive {" +
        "          height: auto !important;" +
        "          max-width: 100% !important;" +
        "          width: auto !important; " +
        "        }" +
        "      }" +
        "      /* -------------------------------------" +
        "          PRESERVE THESE STYLES IN THE HEAD" +
        "      ------------------------------------- */" +
        "      @media all {" +
        "        .ExternalClass {" +
        "          width: 100%; " +
        "        }" +
        "        .ExternalClass," +
        "        .ExternalClass p," +
        "        .ExternalClass span," +
        "        .ExternalClass font," +
        "        .ExternalClass td," +
        "        .ExternalClass div {" +
        "          line-height: 100%; " +
        "        }" +
        "        .apple-link a {" +
        "          color: inherit !important;" +
        "          font-family: inherit !important;" +
        "          font-size: inherit !important;" +
        "          font-weight: inherit !important;" +
        "          line-height: inherit !important;" +
        "          text-decoration: none !important; " +
        "        }" +
        "        .btn-primary table td:hover {" +
        "          background-color: #34495e !important; " +
        "        }" +
        "        .btn-primary a:hover {" +
        "          background-color: #34495e !important;" +
        "          border-color: #34495e !important; " +
        "        } " +
        "      }" +
        "    </style>" +
        "  </head>";
        private static string bodyRegistro = "  <body class=\"\">" +
        "    <span class=\"preheader\">Aqui esta tu token para completar tu registro en DAMnificus.</span>" +
        "    <table role=\"presentation\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"body\">" +
        "      <tr>" +
        "        <td>&nbsp;</td>" +
        "        <td class=\"container\">" +
        "          <div class=\"content\">" +
        "            <!-- START CENTERED WHITE CONTAINER -->" +
        "            <table role=\"presentation\" class=\"main\">" +
        "              <!-- START MAIN CONTENT AREA -->" +
        "              <tr>" +
        "                <td class=\"wrapper\">" +
        "                  <table role=\"presentation\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\">" +
        "                    <tr>" +
        "                      <td>" +
        "                        <p>Bienvenido a DAMnificus,</p>" +
        "                        <p>Siempre es de agrado recibir un nuevo miembro en nuestra comunidad. Esperamos que disfrutes de nuestro servicio." +
                                    "A continuación te damos el token que debes copiar en la aplicación para finalizar el registro:</p>" +
        "                        <p><b>";
        
        private static string bodyReporte = "  <body class=\"\">" +
        "    <span class=\"preheader\">Reporte de usuario para la App DAMnificus</span>" +
        "    <table role=\"presentation\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"body\">" +
        "      <tr>" +
        "        <td>&nbsp;</td>" +
        "        <td class=\"container\">" +
        "          <div class=\"content\">" +
        "            <!-- START CENTERED WHITE CONTAINER -->" +
        "            <table role=\"presentation\" class=\"main\">" +
        "              <!-- START MAIN CONTENT AREA -->" +
        "              <tr>" +
        "                <td class=\"wrapper\">" +
        "                  <table role=\"presentation\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\">" +
        "                    <tr>" +
        "                      <td>" +
        "                        <p>Reporte recibido,</p>" +
        "                        <p>Hemos recibido un reporte de un usuario sobre la aplicación, a continuación se expone el mensaje:</p>" +
        "                        <p><b>";
        
        private static string bodyPassPerdida = "  <body class=\"\">" +
        "    <span class=\"preheader\">Aqui esta tu token para completar cambiar tu contraseña en DAMnificus.</span>" +
        "    <table role=\"presentation\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"body\">" +
        "      <tr>" +
        "        <td>&nbsp;</td>" +
        "        <td class=\"container\">" +
        "          <div class=\"content\">" +
        "            <!-- START CENTERED WHITE CONTAINER -->" +
        "            <table role=\"presentation\" class=\"main\">" +
        "              <!-- START MAIN CONTENT AREA -->" +
        "              <tr>" +
        "                <td class=\"wrapper\">" +
        "                  <table role=\"presentation\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\">" +
        "                    <tr>" +
        "                      <td>" +
        "                        <p>Bienvenido a DAMnificus,</p>" +
        "                        <p>Si no has solicitado cambiar la contraseña de la app DAMnificus ignora este mensaje y bórralo." +
                                    "Si por el contrario sí que has solicitado cambiar la contraseña de tu cuenta aquí tienes el token que necesitas:</p>" +
        "                        <p><b>";
        private static string bodyFinal = "</b></p>" +
        "                        <p>Un saludo.</p>" +
        "                      </td>" +
        "                    </tr>" +
        "                  </table>" +
        "                </td>" +
        "              </tr>" +
        "            <!-- END MAIN CONTENT AREA -->" +
        "            </table>" +
        "            <!-- END CENTERED WHITE CONTAINER -->" +
        "            <!-- START FOOTER -->" +
        "            <div class=\"footer\">" +
        "              <table role=\"presentation\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\">" +
        "                <tr>" +
        "                  <td class=\"content-block\">" +
        "                    <span class=\"apple-link\">Proyecto DAM 2019 IES Melchor Gaspar de Jovellanos, Fuenlabrada</span>" +
        "                    <br> ¿Tienes problemas con el token? <a href=\"https://i.gifer.com/7jwu.gif\" > Pulsa aquí</a>." +
        "                  </td>" +
        "                </tr>" +
        "                <tr>" +
        "                  <td class=\"content-block powered-by\">" +
        "                    Powered by <a href=\"http://www.losmasviciosos.es\">Rubén, Antonio y Valentín</a>." +
        "                  </td>" +
        "                </tr>" +
        "              </table>" +
        "            </div>" +
        "            <!-- END FOOTER -->" +
        "          </div>" +
        "        </td>" +
        "        <td>&nbsp;</td>" +
        "      </tr>" +
        "    </table>" +
        "  </body>" +
        "</html>";
    }
}
