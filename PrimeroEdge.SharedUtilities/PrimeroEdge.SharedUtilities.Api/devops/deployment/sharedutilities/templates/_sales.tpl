{{- define "sales.env" }}
          - name: "ASPNETCORE_ENVIRONMENT"
            value: {{ .Values.aspnetcore }}
          - name: "AppEnv"
            value: {{ .Values.namespace }}
          - name: "ServiceSettings__SourceSettings__ConnectionString"
            valueFrom:
              secretKeyRef:
                name: servicesettingssourcesettingsconnectionstring
                key: servicesettingssourcesettingsconnectionstring
          - name: "RedisClientConfiguration__ConnectionString"
            valueFrom:
              secretKeyRef:
                name: redisclientconfigurationconnectionstring
                key: redisclientconfigurationconnectionstring
          - name: "ConnectionStrings__ADMINISTRATION"
            valueFrom:
              secretKeyRef:
                name: connectionstringsadministration
                key: connectionstringsadministration
          - name: "LogSettings__LogProvider__LogConfiguration"
            valueFrom:
              secretKeyRef:
                name: logsettingslogproviderlogconfiguration
                key: logsettingslogproviderlogconfiguration
          - name: "CouchbaseSettings__Host"
            valueFrom:
              secretKeyRef:
                name: couchbasesettingshost 
                key: couchbasesettingshost
          - name: "CouchbaseSettings__UserName"
            valueFrom:
              secretKeyRef:
                name: couchbasesettingsusername
                key: couchbasesettingsusername
          - name: "CouchbaseSettings__Password"
            valueFrom:
              secretKeyRef:
                name: couchbasesettingspassword
                key: couchbasesettingspassword
          - name: "AuditCouchbaseSettings__Host"
            valueFrom:
              secretKeyRef:
                name: auditcouchbasesettingshost
                key: auditcouchbasesettingshost
          - name: "AuditCouchbaseSettings__UserName"
            valueFrom:
              secretKeyRef:
                name: auditcouchbasesettingsusername
                key: auditcouchbasesettingsusername
          - name: "AuditCouchbaseSettings__Password"
            valueFrom:
              secretKeyRef:
                name: auditcouchbasesettingspassword
                key: auditcouchbasesettingspassword
          - name: "RedisClientConfiguration__Server"
            valueFrom:
              secretKeyRef:
                name: redisclientconfigurationserver
                key: redisclientconfigurationserver
          - name: "RedisClientConfiguration__Password"
            valueFrom:
              secretKeyRef:
                name: redisclientconfigurationpassword
                key: redisclientconfigurationpassword
          - name: "AzureBlobStorageCredential__Key"
            valueFrom:
              secretKeyRef:
                name: azureblobstoragecredentialkey
                key: azureblobstoragecredentialkey
          - name: "AzureBlobStorageCredential__ConnectionString"
            valueFrom:
              secretKeyRef:
                name: azureblobstoragecredentialconnectionstring
                key: azureblobstoragecredentialconnectionstring 
{{- end }}