pipeline {
    agent any
    
    tools {
        dotnet 'dotnet-sdk-8.0' 
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
        
        // 2. PROBAR - Ejecuta TODAS las pruebas del proyecto UnitTest
        stage('Test') {
            steps {
                echo 'Ejecutando pruebas unitarias...'
                dir('UnitTest/Controller') {
                    bat 'dotnet test --verbosity normal'
                }
            }
        }
        
        // 3. PUBLICAR
        stage('Publish') {
            steps {
                echo 'Publicando API...'
                dir('ApiRestSistemaVentas') {
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
            echo 'Pipeline exitoso - Todas las pruebas pasaron correctamente'
        }
        failure {
            echo 'Pipeline fallido - Revisar pruebas unitarias'
        }
    }
}