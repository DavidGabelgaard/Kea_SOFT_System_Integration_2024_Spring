import dotenv from "dotenv";
import express from "express";
import Stripe from "stripe";

dotenv.config();
const app = express();
app.use(express.json());
app.use(express.static("public"));

const stripe = Stripe(process.env.STRIPE_PRIVATE_KEY);

const storeItems = new Map([
    [1, {priceInCents: 100, name: "Lego"}],
    [2, {priceInCents: 200, name: "Dublo"}],
]);

const PORT = 3000;
app.listen(PORT, () => console.log("Server is running on port", PORT));


app.post('/create-checkout-session', async (req, res) => {
    try{
        const session = await stripe.checkout.sessions.create({
            payment_method_types: ['card'],
            mode: 'payment',
            line_items: req.body.items.map(item => {
                const storeItem = storeItems.get(item.id)
                return {
                    price_data: {
                        currency: 'usd',
                        product_data: {
                           name: storeItem.name 
                        },
                        unit_amount: storeItem.priceInCents
                    },
                    quantity: item.quantity
                }
            }),
            success_url: `${process.env.SERVER_URL}/success.html`,
            cancel_url: `${process.env.SERVER_URL}/cancel.html`
        })

        res.json({ url: session.url })
    } catch (e) {
        res.status(500).json({ error: e.message })
    }
})

//Testing payment success use 42 repeated, date anything in the future, sid 424, name on card 42 repeated, and zip code 424242 