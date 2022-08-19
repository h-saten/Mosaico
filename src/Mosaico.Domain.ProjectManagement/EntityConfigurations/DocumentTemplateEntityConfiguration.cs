using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Core.EntityFramework;
using Mosaico.Domain.ProjectManagement.Entities;
using System;

namespace Mosaico.Domain.ProjectManagement.EntityConfigurations
{
    public class DocumentTemplateEntityConfiguration : EntityConfigurationBase<DocumentTemplate>
    {
        protected override string TableName => Constants.Tables.DocumentTemplates;
        protected override string Schema => Constants.Schema;

        public override void Configure(EntityTypeBuilder<DocumentTemplate> builder)
        {
            base.Configure(builder);
 
            builder.HasData(new DocumentTemplate
            {
                Id = new Guid("39e51fcf-afa2-48f6-9a65-ff03f63bf939"),
                Language = Mosaico.Base.Constants.Languages.English,
                IsEnabled = true,
                TemplateVersion = "0.1",
                Key = Constants.DocumentTypes.Whitepaper,
                Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Suspendisse non lectus nec quam cursus eleifend pulvinar non ligula. Maecenas neque velit, sagittis sit amet tincidunt sed, auctor vel nulla. Vestibulum efficitur tempus quam, dignissim consequat dui viverra eget. Pellentesque ullamcorper, nunc a luctus tincidunt, orci urna venenatis velit, nec accumsan odio massa a turpis. Curabitur nec iaculis arcu, sed lobortis lectus. Sed scelerisque diam sed erat scelerisque elementum. Ut pretium nunc justo, vitae ultricies justo commodo vel."
            });
            builder.HasData(new DocumentTemplate
            {
                Id = new Guid("d8a8944c-7a06-46c5-adc4-ae38cfae0da7"),
                Language = Mosaico.Base.Constants.Languages.Polish,
                IsEnabled = true,
                TemplateVersion = "0.1",
                Key = Constants.DocumentTypes.Whitepaper,
                Content = "Cras varius mattis laoreet. Morbi maximus placerat diam, in vulputate massa maximus non. In vitae venenatis risus, vitae luctus lacus. Vestibulum est sapien, interdum sed metus at, blandit volutpat odio. Curabitur nec nisi quam. Nullam sed dolor in nisi maximus sagittis. Quisque volutpat dignissim vehicula. Praesent tortor nibh, hendrerit sed laoreet sed, convallis ac neque. Ut vel erat eu arcu rutrum tempor vel tempor est. Quisque mollis nunc vitae fringilla ultricies. Praesent id justo orci. Duis sit amet dolor quis leo lacinia commodo. Ut ut pretium erat."
            });
            builder.HasData(new DocumentTemplate
            {
                Id = new Guid("29a30233-58ea-47c1-a5a4-f979e88c7bad"),
                Language = Mosaico.Base.Constants.Languages.English,
                IsEnabled = true,
                TemplateVersion = "0.1",
                Key = Constants.DocumentTypes.TermsAndConditions,
                Content = "Ut quis neque nec odio porta ultrices a vel magna. Pellentesque fringilla suscipit quam nec iaculis. Pellentesque scelerisque, sapien sed eleifend maximus, nisi augue commodo ante, eget finibus lorem purus vitae est. Vivamus euismod fermentum eros, sed vestibulum justo tempor vel. Pellentesque ac fringilla tortor. Nam urna enim, mattis molestie tortor sit amet, eleifend commodo erat. Sed tempor luctus enim, et efficitur mi cursus sit amet. Aliquam scelerisque, est ac posuere viverra, enim velit eleifend justo, vel tristique leo lorem eget enim. Curabitur elit mi, laoreet a mi sed, luctus pellentesque augue. Nunc tellus elit, pretium ac viverra sit amet, commodo vel magna. Donec vitae ultricies arcu."
            });
            builder.HasData(new DocumentTemplate
            {
                Id = new Guid("ec5edbdd-3f32-42d8-9a52-0e6a5753d0cd"),
                Language = Mosaico.Base.Constants.Languages.Polish,
                IsEnabled = true,
                TemplateVersion = "0.1",
                Key = Constants.DocumentTypes.TermsAndConditions,
                Content = "Sed sollicitudin ornare sem, vitae pellentesque odio. Sed vel elit feugiat, ornare quam dictum, varius lectus. Suspendisse potenti. Pellentesque lobortis eros nec purus consequat tristique. Sed sodales gravida ex ac condimentum. Aenean sit amet neque vel mi sodales volutpat. Aliquam neque urna, pretium ut eros non, ullamcorper dictum nunc. Pellentesque semper, justo ut tincidunt tristique, diam metus posuere augue, sed finibus quam arcu id libero. Nulla vestibulum scelerisque purus id placerat. Integer feugiat viverra consectetur. Nulla sit amet auctor elit, eu tempus velit. Nulla vitae lacus aliquet, aliquet dolor et, vulputate dolor. Etiam arcu leo, lobortis quis massa consectetur, tincidunt accumsan velit."
            });
            builder.HasData(new DocumentTemplate
            {
                Id = new Guid("69e52c4a-4207-475f-b8e2-551c687d213f"),
                Language = Mosaico.Base.Constants.Languages.English,
                IsEnabled = true,
                TemplateVersion = "0.1",
                Key = Constants.DocumentTypes.PrivacyPolicy,
                Content = "Pellentesque ut ipsum a est tincidunt ornare. In euismod ullamcorper venenatis. Nullam at justo nisl. Nunc sed fringilla mi, laoreet facilisis odio. Fusce in neque est. Cras lacus nunc, vulputate ac suscipit eget, volutpat at ante. Sed in justo tempor, venenatis magna a, suscipit orci. In sit amet condimentum elit. Mauris vitae nunc at arcu tincidunt tristique. In hac habitasse platea dictumst. In ac metus sed risus dignissim ullamcorper."
            });
            builder.HasData(new DocumentTemplate
            {
                Id = new Guid("6b2390d7-6c25-42de-83a8-b81b025ecc4e"),
                Language = Mosaico.Base.Constants.Languages.Polish,
                IsEnabled = true,
                TemplateVersion = "0.1",
                Key = Constants.DocumentTypes.PrivacyPolicy,
                Content = "Donec justo enim, euismod fringilla velit faucibus, commodo ornare libero. Integer mattis sem quis tellus hendrerit molestie non eget nisl. Nulla a pulvinar felis. Curabitur facilisis quam in ante auctor, nec euismod mi sagittis. Maecenas eget leo vitae nulla pulvinar ultrices. Mauris iaculis fringilla lacinia. Integer et semper mauris. Duis dapibus enim sit amet est efficitur condimentum. Vivamus varius efficitur sapien, eget semper arcu volutpat nec. Praesent efficitur accumsan orci id faucibus. Etiam convallis velit eu odio bibendum, non dignissim turpis gravida. Proin ultricies eros in diam iaculis, ac rutrum nulla sollicitudin. Fusce eu nisi tortor. Nulla hendrerit erat vitae mi efficitur, luctus pulvinar purus pharetra. Cras at imperdiet mauris. Duis sed porta neque."
            });
        }
    }
}