const mongoose = require('mongoose');

const CurrencySchema = new mongoose.Schema({
    base: { type: String, default: 'USD' },
    rates: {
        GTQ: Number,
        USD: Number,
        EUR: Number
    },
    updatedAt: { type: Date, default: Date.now }
});

module.exports = mongoose.model('Currency', CurrencySchema);