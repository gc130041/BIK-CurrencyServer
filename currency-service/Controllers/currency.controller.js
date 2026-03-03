const Currency = require('../Models/currency.model.js'); 

const convert = async (req, res) => {
    try {
        const { amount, from, to } = req.body;

        if (amount === undefined || amount <= 0) {
            return res.status(400).json({ 
                error: "Validación de Entrada: El monto debe ser un número positivo y mayor a cero" 
            });
        }

        const data = await Currency.findOne({ base: 'USD' });
        if (!data) return res.status(500).json({ error: "Tasas no disponibles en la base de datos local" });

        const rates = data.rates;

        if (!rates[from] || !rates[to]) {
            return res.status(400).json({ 
                error: `Moneda no soportada. Las monedas válidas son: ${Object.keys(rates).join(', ')}` 
            });
        }

        const amountInUSD = amount / rates[from];
        const convertedAmount = amountInUSD * rates[to];
        const currentRate = rates[to] / rates[from];

        res.json({
            originalAmount: amount,
            convertedAmount: parseFloat(convertedAmount.toFixed(2)),
            rate: parseFloat(currentRate.toFixed(4))
        });

    } catch (error) {
        console.error("Error en conversion:", error);
        res.status(500).json({ error: "Error interno en el servidor durante la conversión." });
    }
};

module.exports = { convert };