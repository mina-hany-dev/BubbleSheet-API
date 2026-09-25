using BubleSheet.Services.Interfaces;
using Domain.bublesheet.Entities;
using Domain.bublesheet.Interfaces;
using BCrypt.Net;

namespace BubleSheet.Services.Implementation
{
    public class ResetPasswordService(IBunnyStorageService bunnyStorageService,IUnitOfWork unitOfWork,IResetPassword resetPassword,IAccount account,IEmailService emailService) : IResetPasswordService
    {
        private readonly IResetPassword _resetPassword = resetPassword;
        private readonly IEmailService _emailService = emailService;
        private readonly IAccount _account = account;
        private readonly IBunnyStorageService _bunnyServiceStorage = bunnyStorageService;
        private readonly IUnitOfWork _unitofwork = unitOfWork;
        public async Task CreateNewOTP(string studentEmail)
        {
            var student = await _account.GetStudentByEmailAsync(studentEmail);

            if (student == null)
                return;

            var oldResetPassword = await _resetPassword.GetAsync(student.StudentId);

            if (oldResetPassword != null)
            {
                await _resetPassword.RemoveAsync(student.StudentId);
                await _unitofwork.SaveChangesAsync();
            }
            
            var otp = Random.Shared.Next(100000, 999999).ToString();

            var now = DateTime.UtcNow;

            var resetPassword = new ResetPassword(student.StudentId,otp,now, now.AddMinutes(5));
            await _resetPassword.AddAsync(resetPassword);

            string Link = _bunnyServiceStorage.GenerateSecureUrl("https://backer.b-cdn.net/Basics/logo.png",5);
            var htmlBody = $@"
<!DOCTYPE html>
<html dir=""rtl"" lang=""ar"">

<head>
  <meta charset=""UTF-8"" />
  <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"" />
  <title>رمز التحقق - Bubble Sheet</title>

  <style>
    body,
    table,
    td {{
      -webkit-text-size-adjust: 100%;
      -ms-text-size-adjust: 100%;
    }}

    table,
    td {{
      mso-table-lspace: 0pt;
      mso-table-rspace: 0pt;
    }}

    img {{
      border: 0;
      line-height: 100%;
      outline: none;
      text-decoration: none;
      -ms-interpolation-mode: bicubic;
    }}

    body {{
      margin: 0;
      padding: 0;
      width: 100% !important;
      height: 100% !important;
    }}

    @media only screen and (max-width: 480px) {{
      .container {{
        width: 100% !important;
      }}

      .p-mobile {{
        padding-left: 20px !important;
        padding-right: 20px !important;
      }}

      .otp-box {{
        font-size: 28px !important;
        letter-spacing: 5px !important;
      }}

      .hero-logo {{
        width: 96px !important;
        height: 96px !important;
      }}
    }}
  </style>
</head>

<body style=""margin:0; padding:0; background-color:#eaf6ff; font-family: 'Almarai', 'Tahoma', sans-serif;"">

  <div style=""display:none; max-height:0; overflow:hidden; opacity:0;"">
    رمز التحقق الخاص بك لإعادة تعيين كلمة المرور في Bubble Sheet
  </div>

  <table role=""presentation"" width=""100%"" cellpadding=""0"" cellspacing=""0""
    style=""background-color:#eaf6ff; background-image: linear-gradient(160deg, #eaf6ff 0%, #f5f0ff 35%, #e8f8ff 70%, #f0f0ff 100%);"">

    <tr>
      <td align=""center"" style=""padding: 40px 16px;"">

        <table role=""presentation"" class=""container"" width=""440"" cellpadding=""0"" cellspacing=""0""
          style=""width:440px; max-width:100%; background-color:#FFFFFF; border-radius:24px; overflow:hidden; border:1px solid #BFEFFF; box-shadow: 0 12px 40px rgba(23,45,157,0.10);"">

          <!-- Logo Hero Banner -->
          <tr>
            <td align=""center""
              style=""padding: 44px 32px 38px; background-color:#0f1c5c; background-image: linear-gradient(135deg, #172D9D 0%, #0f1c5c 55%, #00A9F2 130%);"">

              <img src=""{Link}""
                alt=""Bubble Sheet""
                class=""hero-logo""
                width=""120""
                height=""120""
                style=""display:block; width:120px; height:120px; max-width:100%; border-radius:28px; border:0; outline:none; text-decoration:none; box-shadow:0 10px 30px rgba(0,0,0,0.25);"" />

              <table role=""presentation"" cellpadding=""0"" cellspacing=""0"" style=""margin-top:16px;"">
                <tr>
                  <td align=""center""
                    style=""font-family:'Tahoma', sans-serif; font-weight:bold; font-size:19px; color:#FFFFFF; letter-spacing:0.3px;"">
                    بابل شيت
                  </td>
                </tr>

                <tr>
                  <td align=""center""
                    style=""font-family:'Tahoma', sans-serif; font-size:10.5px; color:#BFE0FF; letter-spacing:3px; padding-top:4px; direction:ltr;"">
                    BUBBLE SHEET
                  </td>
                </tr>
              </table>

            </td>
          </tr>

          <!-- Title -->
          <tr>
            <td align=""center"" class=""p-mobile"" style=""padding: 32px 32px 0;"">
              <h1 style=""margin:0; font-family:'Tahoma', sans-serif; font-weight:bold; font-size:21px; color:#0f1c5c;"">
                رمز التحقق الخاص بك
              </h1>
            </td>
          </tr>

          <!-- Description -->
          <tr>
            <td align=""center"" class=""p-mobile"" style=""padding: 12px 32px 0;"">
              <p style=""margin:0; font-family:'Tahoma', sans-serif; font-size:13.5px; line-height:1.9; color:#6878a8;"">
                استخدم الرمز التالي لإعادة تعيين كلمة المرور الخاصة بحسابك في Bubble Sheet.
              </p>
            </td>
          </tr>

          <!-- OTP Code -->
          <tr>
            <td align=""center"" class=""p-mobile"" style=""padding: 30px 32px 6px;"">

              <table role=""presentation"" cellpadding=""0"" cellspacing=""0"" width=""100%"">
                <tr>
                  <td
                    style=""background-color:#F0F9FF; border:1.5px solid #BFEFFF; border-radius:18px; padding:22px 16px;"">

                    <table role=""presentation"" cellpadding=""0"" cellspacing=""0"" align=""center"">
                      <tr>

                        <td>
                          <table role=""presentation"" cellpadding=""0"" cellspacing=""0"" style=""direction:ltr;"">
                            <tr>

                              <td style=""width:8px;"">&nbsp;</td>

                              <td style=""width:38px; height:48px; background-color:#FFFFFF; border:1.5px solid #BFEFFF; border-radius:10px; box-shadow:0 2px 6px rgba(23,45,157,0.06); text-align:center; vertical-align:middle; font-family:'Courier New', monospace; font-weight:bold; font-size:24px; color:#172D9D;"">
                                {otp.ToString()[0]}
                              </td>

                              <td style=""width:8px;"">&nbsp;</td>

                              <td style=""width:38px; height:48px; background-color:#FFFFFF; border:1.5px solid #BFEFFF; border-radius:10px; box-shadow:0 2px 6px rgba(23,45,157,0.06); text-align:center; vertical-align:middle; font-family:'Courier New', monospace; font-weight:bold; font-size:24px; color:#172D9D;"">
                                {otp.ToString()[1]}
                              </td>

                              <td style=""width:8px;"">&nbsp;</td>

                              <td style=""width:38px; height:48px; background-color:#FFFFFF; border:1.5px solid #BFEFFF; border-radius:10px; box-shadow:0 2px 6px rgba(23,45,157,0.06); text-align:center; vertical-align:middle; font-family:'Courier New', monospace; font-weight:bold; font-size:24px; color:#172D9D;"">
                                {otp.ToString()[2]}
                              </td>

                              <td style=""width:8px;"">&nbsp;</td>

                              <td style=""width:38px; height:48px; background-color:#FFFFFF; border:1.5px solid #BFEFFF; border-radius:10px; box-shadow:0 2px 6px rgba(23,45,157,0.06); text-align:center; vertical-align:middle; font-family:'Courier New', monospace; font-weight:bold; font-size:24px; color:#172D9D;"">
                                {otp.ToString()[3]}
                              </td>

                              <td style=""width:8px;"">&nbsp;</td>

                              <td style=""width:38px; height:48px; background-color:#FFFFFF; border:1.5px solid #BFEFFF; border-radius:10px; box-shadow:0 2px 6px rgba(23,45,157,0.06); text-align:center; vertical-align:middle; font-family:'Courier New', monospace; font-weight:bold; font-size:24px; color:#172D9D;"">
                                {otp.ToString()[4]}
                              </td>

                              <td style=""width:8px;"">&nbsp;</td>

                              <td style=""width:38px; height:48px; background-color:#FFFFFF; border:1.5px solid #BFEFFF; border-radius:10px; box-shadow:0 2px 6px rgba(23,45,157,0.06); text-align:center; vertical-align:middle; font-family:'Courier New', monospace; font-weight:bold; font-size:24px; color:#172D9D;"">
                                {otp.ToString()[5]}
                              </td>

                              <td style=""width:8px;"">&nbsp;</td>

                            </tr>
                          </table>
                        </td>

                      </tr>
                    </table>

                  </td>
                </tr>
              </table>

              <p style=""margin:10px 0 0; font-family:'Tahoma', sans-serif; font-size:11px; color:#6878a8;"">
                استخدم الرمز لإعادة تعيين كلمة المرور
              </p>

            </td>
          </tr>

          <!-- Expiry -->
          <tr>
            <td align=""center"" class=""p-mobile"" style=""padding:18px 32px 0;"">
              <table role=""presentation"" cellpadding=""0"" cellspacing=""0"">
                <tr>
                  <td style=""background-color:#FFF4E5; border:1px solid #FFE0B2; border-radius:20px; padding:7px 16px;"">

                    <p style=""margin:0; font-family:'Tahoma', sans-serif; font-size:12px; color:#B5691A;"">
                      &#9201; ينتهي الرمز خلال
                      <span style=""font-weight:bold; direction:ltr; display:inline-block;"">05:00</span>
                      دقائق
                    </p>

                  </td>
                </tr>
              </table>
            </td>
          </tr>

          <!-- Divider -->
          <tr>
            <td class=""p-mobile"" style=""padding:30px 32px 0;"">
              <div style=""height:1px; background-color:#BFEFFF; font-size:0; line-height:0;"">
                &nbsp;
              </div>
            </td>
          </tr>

          <!-- Security -->
          <tr>
            <td align=""center"" class=""p-mobile"" style=""padding:22px 32px 38px;"">

              <table role=""presentation"" cellpadding=""0"" cellspacing=""0"" width=""100%"">
                <tr>

                  <td style=""background-color:#F7F9FC; border-radius:12px; padding:16px 18px;"">

                    <p
                      style=""margin:0; font-family:'Tahoma', sans-serif; font-size:12px; line-height:1.9; color:#6878a8;"">
                      &#128274; إذا لم تطلب إعادة تعيين كلمة المرور، يمكنك تجاهل هذه الرسالة بأمان.
                      لن يتم تغيير كلمة مرورك ما لم تستخدم الرمز أعلاه.
                    </p>

                  </td>

                </tr>
              </table>

            </td>
          </tr>

        </table>

        <!-- Footer -->
        <table role=""presentation"" class=""container"" width=""440"" cellpadding=""0"" cellspacing=""0"">

          <tr>
            <td align=""center"" style=""padding:24px 16px 0;"">

              <p style=""margin:0; font-family:'Tahoma', sans-serif; font-size:11.5px; color:#8290b8;"">
                © 2026 Bubble Sheet. جميع الحقوق محفوظة.
              </p>

            </td>
          </tr>

        </table>

      </td>
    </tr>
  </table>

</body>
</html>
";

            await _emailService.SendEmailAsync(
                studentEmail,
                "Reset Password OTP",
                htmlBody
            );
        }
        public async Task<bool> VerifyOTP(string studentEmail, string otp)
        {
            var student = await _account.GetStudentByEmailAsync(studentEmail);

            if (student == null)
                return false;

            var resetPassword = await _resetPassword.GetAsync(student.StudentId);

            if (resetPassword == null)
                return false;

            var now = DateTime.UtcNow;

            if (resetPassword.ExpiredAt <= now)
                return false;

            if (resetPassword.CodeOTP != otp)
                return false;

            if (resetPassword.IsUsed == true)
                return false;
            resetPassword.UpdateIsUsed();
            await _unitofwork.SaveChangesAsync();
            return true;
        }
        public async Task UpdatePassword(
     string studentEmail,
     string newPassword)
        {
            var student = await _account.GetStudentByEmailAsync(studentEmail);
            if (student == null)
                return;
            var resetPassword = await _resetPassword.GetAsync(student.StudentId);
            if (resetPassword == null || !resetPassword.IsUsed) return;
            student.ChangePassword(BCrypt.Net.BCrypt.HashPassword(newPassword));
            await _unitofwork.SaveChangesAsync();
        }
    }
}
