const express = require('express');
const puppeteer = require('puppeteer');
const PuppeteerReport = require('puppeteer-report');
const path = require('path');

const app = express();
const port = 3000;

app.use(express.json());
app.use(express.static(path.join(__dirname, '../public'))); // Servir archivos estáticos desde la carpeta 'public'

app.post('/generate-pdf', async (req, res) => {
    const { type } = req.body;
    let content = '';

    switch (type) {
        case 'receta':
            content = '<h1>Receta</h1><p>Contenido del PDF de Receta.</p>';
            break;
        case 'justificacion':
            content = '<h1>Certificado Médico</h1><p>Contenido del PDF de Certificado Médico.</p>';
            break;
        case 'formatoConsulta':
            content = '<h1>Formato de Consulta</h1><p>Contenido del PDF de Formato de Consulta.</p>';
            break;
        case 'laboratorio':
            content = '<h1>Laboratorio</h1><p>Contenido del PDF de Laboratorio.</p>';
            break;
        case 'imagen':
            content = '<h1>Imagen (Exámen)</h1><p>Contenido del PDF de Imagen.</p>';
            break;
        default:
            res.status(400).send('Tipo de documento no válido');
            return;
    }

    try {
        const browser = await puppeteer.launch();
        const page = await browser.newPage();

        const report = new PuppeteerReport(page, {
            output: 'report.pdf',
            template: {
                content: content
            }
        });

        await report.generate();
        await browser.close();

        res.sendFile(path.join(__dirname, 'report.pdf'));
    } catch (error) {
        res.status(500).send('Error al generar el PDF');
    }
});

app.listen(port, () => {
    console.log(`Server running at http://localhost:${port}`);
});
