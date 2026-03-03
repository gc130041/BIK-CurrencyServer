const cron = require('node-cron');
const { refreshExchangeRates } = require('../Services/currency.service.js');

const initCurrencyCron = () => {
    cron.schedule('0 */12 * * *', () => {
        refreshExchangeRates();
    });
    console.log("Tarea programada de divisas iniciada");
};

module.exports = initCurrencyCron;