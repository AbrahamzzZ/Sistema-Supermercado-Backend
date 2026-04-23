pipeline {
    agent any
    
    tools {
        dotnetsdk 'dotnet-sdk-8.0'   // ← CAMBIADO: dotnet → dotnetsdk
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
                dir('UnitTests') {   // ← CAMBIADO: UnitTest → UnitTests (con 's')
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
            echo 'Pipeline exitoso - Todas las pruebas pasaron correctamente'
        }
        failure {
            echo 'Pipeline fallido - Revisar pruebas unitarias'
        }
    }
}