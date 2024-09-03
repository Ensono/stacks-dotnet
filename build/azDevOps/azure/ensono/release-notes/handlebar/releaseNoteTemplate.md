___

## {{buildDetails.buildNumber}}

**🏗 Build [{{buildDetails.buildNumber}}]({{buildDetails._links.web.href}})**
{{#forEach this.pullRequests}}
**Associated PR:** [{{this.title}}]({{replace (replace this.url "_apis/git/repositories" "_git") "pullRequests" "pullRequest"}}) - Created By: {{this.createdBy.displayName}}
{{/forEach}}

{{#if this.workItems.length}}
**🔧 {{inflect this.workItems.length "Associated Work Item" "Associated Work Items" true}}**
{{#forEach this.workItems}}
- <img style="vertical-align: bottom; height: 22px; padding-bottom: 1px;" src="{{replace (get 'imageUrl' (get 'System.AssignedTo' this.fields)) "ensonodigitaluk.visualstudio.com" "dev.azure.com/ensonodigitaluk"}}" title="{{get 'displayName' (get 'System.AssignedTo' this.fields)}}"/> {{lookup this.fields 'System.WorkItemType'}} {{this.id}}: [{{lookup this.fields 'System.Title'}}]({{replace this.url "_apis/wit/workItems" "_workitems/edit"}})
{{/forEach}}
{{/if}}

{{#with (format_test_results this.tests "Cucumber Test Run") as |formattedTests|}}
{{#if formattedTests.length}}
{{#with (sum_tests formattedTests) as |count|}}
**🧪 {{inflect count "Test" "Tests" true}}**
{{/with}}
| **Feature** | **Tests** | **Passed** | **Errors** |
|-------------|-----------|------------|------------|
{{#forEach formattedTests}}
|{{this.feature}}|{{this.tests}}|{{this.passes}}|{{this.errors}}|
{{/forEach}}
{{/if}}
{{/with}}

{{#with (format_test_results this.tests "Cucumber Test Retries") as |formattedTests|}}
{{#if formattedTests.length}}
{{#with (sum_tests formattedTests) as |count|}}
**⚠️ {{inflect count "Test Retried" "Tests Retried" true}}**
{{/with}}
| **Feature** | **Tests** | **Passed** | **Errors** |
|-------------|-----------|------------|------------|
{{#forEach formattedTests}}
|{{this.feature}}|{{this.tests}}|{{this.passes}}|{{this.errors}}|
{{/forEach}}
{{/if}}
{{/with}}

{{#if this.publishedArtifacts.length}}
{{inflect this.publishedArtifacts.length "Artefact has" "Artefacts have" true}} been created - [View list]({{buildDetails._links.web.href}}&view=artifacts&pathAsName=false&type=publishedArtifacts)
{{#forEach publishedArtifacts}}
- [{{name}}]({{resource.downloadUrl}})
{{/forEach}}
{{/if}}

{{#if this.commits.length}}
**📝 Commits**
{{#forEach this.commits}}
<small><span><a href="{{../buildDetails.repository.url}}/commit/{{id}}">{{truncate id 5}}</a></span> by <b>{{author.displayName}}</b> {{#if (gt changes.length 0)}}({{inflect changes.length "file" "files" true}} changed) {{/if}} - "{{get_only_message_firstline message}}"</small>
{{/forEach}}
{{/if}}
___