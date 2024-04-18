import express from "express";
const app = express();

app.use(express.urlencoded({ extended: true }));

import multer from "multer";
//const upload = multer({ dest: "./uploads" });

const storage = multer.diskStorage({
    destination: (req, file, cb) => {
        console.log(req);
        console.log(file);

        cb(null, './uploads');
    },
    filename: (req, file, cb) => {

        const uniquePrefix = Date.now() + '-' + Math.round(Math.random() * 1E9)
        cb(null, uniquePrefix + "-" + file.originalname ) 
    }
});

function fileFilter(req, file, cb) {
    const allowedTypes = ['image/png', 'image/svg', 'image.jpeg'];

    if (!allowedTypes.includes(file.mimetype)) {
        cb(new Error("File type not allowed: " + file.mimetype), false);
    } else {
        cb(null, true);
    }
} 

const upload = multer({
    storage,
    fileFilter
});

app.post('/fileform', upload.single('file'), (req, res) => {
    console.log(req.body);
    res.send({ });
});


const PORT = process.env.PORT ?? 8080;
app.listen(PORT, () => console.log("Server is running on port", PORT));
