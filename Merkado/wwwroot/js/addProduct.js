let fileInput = document.getElementById("file-input");
let imageContainer = document.getElementById("images");
let numOfFiles = document.getElementById("num-of-files");

function preview() {
    if (fileInput.files.length <= 6)
    {
        imageContainer.innerHTML = "";
        numOfFiles.textContent = `Liczba wybranych zdjęć: ${fileInput.files.length}`;
        for (i of fileInput.files) {
            let reader = new FileReader();
            let figure = document.createElement("figure");
            let figCap = document.createElement("figcaption");
            figCap.innerText = i.name;
            figure.appendChild(figCap);
            reader.onload = () => {
                let img = document.createElement("img");
                img.setAttribute("src", reader.result);
                figure.insertBefore(img, figCap);
            }
            imageContainer.appendChild(figure);
            reader.readAsDataURL(i);
        }
    }
    else
    {
        alert('Możesz dodać maksymalnie 6 zdjęć');
    }
}