function cargarEtiquetasProducto(idPublicacion) {
    const url = `/Account/DevolverEtiquetasPorPublicacion?idPublicacion=${idPublicacion}`;
    const elementoEtiquetas = document.getElementById(`etiquetas-${idPublicacion}`);
    
    if (!elementoEtiquetas) {
        console.warn(`No se encontró el elemento para etiquetas ID: ${idPublicacion}`);
        return;
    }

    elementoEtiquetas.textContent = 'Cargando...';
    fetch(url)
        .then(response => {
            if (!response.ok) {
                throw new Error(`Error HTTP: ${response.status}`);
            }
            return response.json();
        })
        .then(etiquetasArray => {
            console.log(`Etiquetas recibidas para ${idPublicacion}:`, etiquetasArray);
            
            if (etiquetasArray && Array.isArray(etiquetasArray) && etiquetasArray.length > 0) {
                const etiquetasTexto = etiquetasArray.join(' ');
                elementoEtiquetas.textContent = etiquetasTexto;
            } else {
                elementoEtiquetas.textContent = 'Sin etiquetas';
            }
        })
        .catch(error => {
            console.error(`Error al cargar etiquetas para ID ${idPublicacion}:`, error);
            elementoEtiquetas.textContent = 'Error';
        });
}

function cargarTodasLasEtiquetas() {
    const elementosEtiquetas = document.querySelectorAll('[id^="etiquetas-"]');
    
    console.log(`Elementos de etiquetas encontrados: ${elementosEtiquetas.length}`);
    
    elementosEtiquetas.forEach(elemento => {
        const idPublicacion = elemento.id.replace('etiquetas-', '');
        
        if (idPublicacion && !isNaN(idPublicacion)) {
            console.log(`Cargando para ID: ${idPublicacion}`);
            cargarEtiquetasProducto(parseInt(idPublicacion));
        }
    });
}

function validar(elemento) {
    event.preventDefault();
    const confirmacion = confirm("¿Estás seguro de que deseas realizar esta acción?");
    if (confirmacion) {
        const url = elemento.getAttribute('href');
        window.location.href = url;
    }
}

document.addEventListener('DOMContentLoaded', function() {
    console.log('=== INICIANDO CARGA DE ETIQUETAS ===');
    cargarTodasLasEtiquetas();
});
setTimeout(cargarTodasLasEtiquetas, 500);

function cambiarImagen(buttonElement) {
    const imagen = buttonElement.querySelector('img'); 
    const srcActual = imagen.getAttribute('src');
    if (srcActual.endsWith('/imagenes/corazon.png')) {
        imagen.setAttribute('src', '/imagenes/likeado.png'); 
    } else {
        imagen.setAttribute('src', '/imagenes/corazon.png');
    }
}