using Microsoft.AspNetCore.Builder;

namespace Mosaico.API.Base.Extensions
{
    public static class AppBuilderExtensions
    {
        public static void AddHeaderPolicies(this IApplicationBuilder app)
        {
            var policies = new HeaderPolicyCollection()
                .AddFrameOptionsDeny()
                .AddXssProtectionBlock()
                .AddContentTypeOptionsNoSniff()
                .AddReferrerPolicyStrictOriginWhenCrossOrigin()
                .RemoveServerHeader()
                .AddCrossOriginOpenerPolicy(builder =>
                {
                    builder.SameOrigin();
                })
                .AddCrossOriginEmbedderPolicy(builder =>
                {
                    builder.RequireCorp();
                })
                .AddCrossOriginResourcePolicy(builder =>
                {
                    builder.SameOrigin();
                })
                .AddContentSecurityPolicy(builder =>
                {
                    builder.AddObjectSrc().None();
                    builder.AddBlockAllMixedContent();
                    builder.AddImgSrc().Self().From("data:");
                    builder.AddFontSrc().Self();
                    builder.AddStyleSrc().Self().UnsafeInline();
                    builder.AddBaseUri().Self();
                    builder.AddScriptSrc().Self().UnsafeInline(); //.WithNonce();
                    builder.AddFrameAncestors().Self();

                    //TODO:
                    // removed this for demos add this back with explicit redirects for prod
                    // builder.AddFormAction().Self();

                    // builder.AddCustomDirective("require-trusted-types-for", "'script'");
                })
                .RemoveServerHeader()
                .AddPermissionsPolicy(builder =>
                {
                    builder.AddAccelerometer().None();
                    builder.AddAutoplay().None();
                    builder.AddCamera().None();
                    builder.AddEncryptedMedia().None();
                    builder.AddFullscreen().All();
                    builder.AddGeolocation().None();
                    builder.AddGyroscope().None();
                    builder.AddMagnetometer().None();
                    builder.AddMicrophone().None();
                    builder.AddMidi().None();
                    builder.AddPayment().None();
                    builder.AddPictureInPicture().None();
                    builder.AddSyncXHR().None();
                    builder.AddUsb().None();
                })
                .AddStrictTransportSecurityMaxAgeIncludeSubDomains(maxAgeInSeconds: 60 * 60 * 24 * 365);
            
            app.UseSecurityHeaders(policies);
        }
    }
}