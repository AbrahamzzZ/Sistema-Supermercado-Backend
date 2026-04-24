pipeline {
    agent any
    
    tools {
        dotnetsdk 'dotnet-sdk-8.0' 
    }
    
    stages {
        // 1. COMPILAR
        stage('Build') {
            steps {
                echo 'Compilando solución...'
                bat 'dotnet restore'
                bat 'dotnet build --configuration Release'
            }
        }
        
        // 2. PROBAR
        stage('Test') {
            steps {
                echo 'Ejecutando pruebas unitarias...'
                dir('UnitTests') {  
                    bat 'dotnet test --verbosity normal'
                }
            }
        }
        
        // 3. PUBLICAR
        stage('Publish') {
            steps {
                echo 'Publicando API...'
                dir('APIRestSistemaVentas') { 
                    bat 'dotnet publish -c Release -o publish'
                }
            }
        }
        
        // 4. DESPLEGAR
        stage('Deploy') {
            steps {
                echo 'Desplegando...'
                bat 'powershell -File "deploy.ps1"'
            }
        }
    }
    post {
        success {
            echo '✅ Pipeline exitoso - Todas las pruebas pasaron correctamente'
            
            emailext (
                to: 'hermanosfarfan@gmail.com',
                subject: "✅ [EXITO] Pipeline ${env.JOB_NAME} - Build #${env.BUILD_NUMBER}",
                body: """
                    El pipeline se ejecutó correctamente.
                    
                    Detalles:
                    - Proyecto: ${env.JOB_NAME}
                    - Build: ${env.BUILD_NUMBER}
                    - URL: ${env.BUILD_URL}
                    - Fecha: ${new Date()}
                    
                    ✅ Todas las pruebas unitarias pasaron exitosamente.
                    ✅ El despliegue se completó sin errores.
                """
            )
        }
        failure {
            echo '❌ Pipeline fallido - Revisar pruebas unitarias'
            
            emailext (
                to: 'hermanosfarfan@gmail.com',
                subject: "❌ [FALLO] Pipeline ${env.JOB_NAME} - Build #${env.BUILD_NUMBER}",
                body: """
                    El pipeline HA FALLADO. Se requiere atención.
                    
                    Detalles:
                    - Proyecto: ${env.JOB_NAME}
                    - Build: ${env.BUILD_NUMBER}
                    - URL: ${env.BUILD_URL}
                    - Fecha: ${new Date()}
                    
                    ❌ Revisar los logs en Jenkins para identificar el error.
                """
            )
        }
    }
}