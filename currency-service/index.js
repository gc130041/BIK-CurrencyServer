const express = require('express');
const mongoose = require('mongoose');
const initCurrencyCron = require('./Cron/currency.cron.js');
const { convert } = require('./Controllers/currency.controller.js');
const { refreshExchangeRates } = require('./Services/currency.service.js');
require('dotenv').config();

const app = express();
app.use(express.json());

const mongoUri = process.env.MONGODB_URI || 'mongodb://localhost:27017/bik_currencies';
mongoose.connect(mongoUri)
    .then(() => console.log("Conectado a MongoDB"))
    .catch(err => console.error("Error de conexión a MongoDB:", err));

initCurrencyCron();

app.post('/BIK/v1/currencies/convert', convert);

const port = process.env.PORT || 3002;
app.listen(port, () => {
    console.log(`Currency Service ejecutándose en puerto ${port}`);
    refreshExchangeRates();
});