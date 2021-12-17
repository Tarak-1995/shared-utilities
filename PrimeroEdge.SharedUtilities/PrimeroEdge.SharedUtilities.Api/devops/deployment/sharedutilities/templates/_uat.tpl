apiVersion : apps/v1
kind: Deployment
metadata:
  name: sharedutilities
spec:
  replicas: {{ .Values.replicas }}
  selector:
    matchLabels:
      app: sharedutilities
  template:
    metadata:
      labels:
        app: sharedutilities 
      annotations:
        timestamp: "{{ date "20060102150405" .Release.Time }}"
    spec: 
      securityContext:
          runAsNonRoot: true
          runAsUser: 1001
          fsGroup: 1001
      containers:
        - name: sharedutilities 
          image: __imageName__
          imagePullPolicy: Always
          securityContext:
            capabilities:
              drop:
               - ALL
          ports:
          - containerPort: 5000
          resources:
            limits:
              cpu: {{ .Values.limitcpu }}
              memory: {{ .Values.limitmemory }}
            requests:
              cpu: {{ .Values.requestscpu }}
              memory: {{ .Values.requestsmemory }}
          volumeMounts:
          - name: secrets
            mountPath: "/mnt/secrets"
          env:
          {{- if eq .Values.namespace "uat" }}
          {{- template "uat.env" . }}
          {{- else }}
          - name: "ASPNETCORE_ENVIRONMENT"
            value: {{ .Values.aspnetcore }}
          - name: "AppEnv"
            value: {{ .Values.namespace }}
          {{- end }}
      volumes:
        - name: secrets
          csi:
            driver: secrets-store.csi.k8s.io
            readOnly: true
            volumeAttributes:
              secretProviderClass: {{ .Values.secretProviderClass }}
            nodePublishSecretRef:
              name: {{ .Values.secretSPName }}
