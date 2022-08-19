# Resource Manager

Resource manager is required to handle static resourecs, like email templates and translations efficient enough. This implementation uses built-in resource manager functionality of .NET platform. For each resource type there are separate resx files that contain key-value pairs in addition to localization.

# EmailTemplates

Contains map between template name and the location of the html template for each localization.

# EmailSubjects

Contains translations of the subject for each email. Each subject should be queryable with the same key as a corresponding mail template.

# Images

Contains key-value pair (non-localized, which means it uses default locale) for Image keys and their location.


- All resource should be configured to be EmbeddedResources through the context option menu or in csproj.
- By default resource manager searches for resources in the assembly of resource manager (Mosaico.Core.ResourceManager), however you can specify an optional parameter (Assembly) to look for resources somewhere else.
- There is also a Mustasche template engine connected (using HandlebarsDotNet package) which allows semi-programmatic logic within templates and subjects. You can replace placeholders, do conditions and loops. Read more here - https://github.com/Handlebars-Net/Handlebars.Net.
