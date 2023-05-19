{{/*
Expand the name of the chart.
*/}}
{{- define "stacks-dotnet.name" -}}
{{- default .Chart.Name .Values.nameOverride | trunc 63 | trimSuffix "-" }}
{{- end }}

{{/*
Create a default fully qualified app name.
We truncate at 63 chars because some Kubernetes name fields are limited to this (by the DNS naming spec).
If release name contains chart name it will be used as a full name.
*/}}
{{- define "stacks-dotnet.fullname" -}}
{{- if .Values.fullnameOverride }}
{{- .Values.fullnameOverride | trunc 63 | trimSuffix "-" }}
{{- else }}
{{- $name := default .Chart.Name .Values.nameOverride }}
{{- if contains $name .Release.Name }}
{{- .Release.Name | trunc 63 | trimSuffix "-" }}
{{- else }}
{{- printf "%s-%s" .Release.Name $name | trunc 63 | trimSuffix "-" }}
{{- end }}
{{- end }}
{{- end }}

{{/*
Create chart name and version as used by the chart label.
*/}}
{{- define "stacks-dotnet.chart" -}}
{{- printf "%s-%s" .Chart.Name .Chart.Version | replace "+" "_" | trunc 63 | trimSuffix "-" }}
{{- end }}

{{/*
Common labels
*/}}
{{- define "stacks-dotnet.labels" -}}
helm.sh/chart: {{ include "stacks-dotnet.chart" . }}
{{ include "stacks-dotnet.selectorLabels" . }}
{{- if .Chart.AppVersion }}
app.kubernetes.io/version: {{ .Chart.AppVersion | quote }}
{{- end }}
app.kubernetes.io/managed-by: {{ .Release.Service }}
{{- end }}

{{/*
Selector labels
*/}}
{{- define "stacks-dotnet.selectorLabels" -}}
app.kubernetes.io/component: {{ .Values.component }}
app.kubernetes.io/name: {{ .Values.resource_def_name }}
app.kubernetes.io/part-of: {{ .Values.app.project }}
environment: {{ .Values.env }}
{{- end }}

{{/*
Create the name of the service account to use
*/}}
{{- define "stacks-dotnet.serviceAccountName" -}}
{{- if .Values.serviceAccount.create }}
{{- default (include "stacks-dotnet.fullname" .) .Values.serviceAccount.name }}
{{- else }}
{{- default "default" .Values.serviceAccount.name }}
{{- end }}
{{- end }}


{{- define "stacks-dotnet.secretName" -}}
{{- printf "%s-%s" (include "stacks-dotnet.name" .) "secret" }}
{{- end }}

{{/*
Common annotations
*/}}
{{- define "stacks-dotnet.annotations" -}}
{{- if .Chart.AppVersion }}
app.kubernetes.io/version: {{ .Chart.AppVersion | quote }}
{{- end }}
app.kubernetes.io/managed-by: {{ .Release.Service }}
app.kubernetes.io/instance: {{- printf "%s-%s" .Values.app.project .Values.app.name }}
applicationId: {{- printf "%s/%s" .Values.app.project .Values.app.name }}
applicaitonName: {{- printf "%s-%s" .Values.app.project .Values.app.name }}
customerID: {{ .Values.app.company }}
owner: {{- printf "%s/%s" .Values.app.company .Values.app.project }}
projectID: {{ .Values.app.project }}
version: {{ .Chart.AppVersion | quote }}
{{- end }}
