// ==========================================
// CONFIGURACIÓN DE DATOS
// ==========================================
const cities = ["Ambato", "Quito", "Riobamba", "Latacunga", "Puyo"];

// Matriz de distancias
const distances = [
    [0,   120, 60,  45,  100], // Ambato
    [120, 0,   180, 90,  220], // Quito
    [60,  180, 0,   105, 130], // Riobamba
    [45,  90,  105, 0,   145], // Latacunga
    [100, 220, 130, 145, 0]    // Puyo
];

// Coordenadas relativas en la interfaz gráfica (x%, y%)
const cityCoordinates = [
    { name: "Ambato", x: 38, y: 55 },
    { name: "Quito", x: 42, y: 15 },
    { name: "Riobamba", x: 34, y: 75 },
    { name: "Latacunga", x: 40, y: 35 },
    { name: "Puyo", x: 65, y: 65 }
];

// ==========================================
// CLASES DEL DOMINIO (BRANCH AND BOUND)
// Traducción directa de las clases en C#
// ==========================================

class TspState {
    constructor(currentCity, visitedCities) {
        this.currentCity = currentCity;
        this.visitedCities = [...visitedCities]; // Clonar array
    }

    isGoal(totalCities) {
        return this.visitedCities.length === totalCities;
    }
}

class Node {
    constructor(state, parent, cost, depth, action) {
        this.state = state;
        this.parent = parent;
        this.cost = cost;
        this.depth = depth;
        this.action = action;
    }
}

// Emulación simple de PriorityQueue usando sort in situ (suficiente para 5 ciudades)
class SimplePriorityQueue {
    constructor() {
        this.items = [];
    }

    enqueue(element) {
        // En un caso real usar Heap binario, pero para N=5 un sort de array es inmediato
        this.items.push(element);
        this.items.sort((a, b) => {
            if (a.cost === b.cost) {
                return b.depth - a.depth; // Preferir profundidad (menor árbol)
            }
            return a.cost - b.cost; // Mínimo costo primero
        });
    }

    dequeue() {
        return this.items.shift();
    }

    get length() {
        return this.items.length;
    }
}

class BranchAndBoundSolver {
    solve() {
        const pq = new SimplePriorityQueue();
        const totalCities = cities.length;
        
        let bestCost = Infinity;
        let bestGoalNode = null;

        // Iniciar probando desde todas las ciudades de origen posibles
        for (let i = 0; i < totalCities; i++) {
            const initialState = new TspState(i, [i]);
            const root = new Node(initialState, null, 0, 0, `Iniciar en ${cities[i]}`);
            pq.enqueue(root);
        }

        let safetyBreak = 0;

        while (pq.length > 0 && safetyBreak < 100000) {
            safetyBreak++;
            const current = pq.dequeue();

            // PODA
            if (current.cost >= bestCost) {
                continue;
            }

            // META
            if (current.state.isGoal(totalCities)) {
                if (current.cost < bestCost) {
                    bestCost = current.cost;
                    bestGoalNode = current;
                }
                continue;
            }

            // EXPANSION
            for (let nextCity = 0; nextCity < totalCities; nextCity++) {
                if (!current.state.visitedCities.includes(nextCity)) {
                    const addedCost = distances[current.state.currentCity][nextCity];
                    
                    if (addedCost > 0) {
                        const newCost = current.cost + addedCost;
                        
                        // SEGUNDA PODA (A priori)
                        if (newCost < bestCost) {
                            const newVisited = [...current.state.visitedCities, nextCity];
                            const newState = new TspState(nextCity, newVisited);
                            const childNode = new Node(newState, current, newCost, current.depth + 1, `Ir a ${cities[nextCity]}`);
                            
                            pq.enqueue(childNode);
                        }
                    }
                }
            }
        }

        return bestGoalNode; // Último nodo de meta
    }
}

// ==========================================
// CONTROLADOR DE INTERFAZ (DOM)
// ==========================================

document.addEventListener('DOMContentLoaded', () => {
    initUI();
    document.getElementById('startBtn').addEventListener('click', startSolvingAnimation);
});

function initUI() {
    renderMatrix();
    renderCities();
}

function renderMatrix() {
    const table = document.getElementById('matrixTable');
    table.innerHTML = "";
    
    // Header
    let theadRow = document.createElement('tr');
    theadRow.innerHTML = "<th></th>" + cities.map(c => `<th>${c.substring(0,3)}</th>`).join('');
    table.appendChild(theadRow);

    // Rows
    distances.forEach((row, rowIndex) => {
        let tr = document.createElement('tr');
        let html = `<th>${cities[rowIndex].substring(0,3)}</th>`;
        row.forEach((dist, colIndex) => {
            if (rowIndex === colIndex) {
                html += `<td class="diagonal">-</td>`;
            } else {
                html += `<td>${dist}</td>`;
            }
        });
        tr.innerHTML = html;
        table.appendChild(tr);
    });
}

const mapNodes = {}; // Guardará las ref a los div de las ciudades

function renderCities() {
    const mapContainer = document.getElementById('mapContainer');
    // Clear old visual nodes
    mapContainer.querySelectorAll('.city-node').forEach(e => e.remove());

    cityCoordinates.forEach((city, index) => {
        const node = document.createElement('div');
        node.className = 'city-node';
        node.style.left = `${city.x}%`;
        node.style.top = `${city.y}%`;
        node.textContent = city.name;
        node.id = `cityNode_${index}`;
        
        mapContainer.appendChild(node);
        mapNodes[index] = node;
    });
}

async function startSolvingAnimation() {
    const btn = document.getElementById('startBtn');
    btn.disabled = true;
    btn.innerHTML = "Calculando Óptimo...";
    
    clearUIForExecution();

    const statusBadge = document.getElementById('statusBadge');
    statusBadge.className = "status-badge running";
    statusBadge.textContent = "Branch & Bound buscando...";

    // 1. Correr el algoritmo (es sincrónico y rápido en JS)
    const solver = new BranchAndBoundSolver();
    const solutionNode = solver.solve();

    if (!solutionNode) {
        statusBadge.className = "status-badge standby";
        statusBadge.textContent = "No hay solución.";
        btn.disabled = false;
        btn.innerHTML = "Descubrir Ruta Óptima";
        return;
    }

    // 2. Extraer el path leyendo los padres
    let path = [];
    let curr = solutionNode;
    while (curr !== null) {
        path.unshift(curr);
        curr = curr.parent;
    }

    // 3. Animar la respuesta (Simulamos un delay visual "wow factor")
    statusBadge.className = "status-badge success";
    statusBadge.textContent = "¡Ruta Óptima Encontrada!";
    btn.innerHTML = "Animando Ruta...";

    const listContainer = document.getElementById('pathList');
    const svgContainer = document.getElementById('linesContainer');

    for (let i = 0; i < path.length; i++) {
        const nodeData = path[i];
        
        // Agregar Item en la Lista
        const li = document.createElement('li');
        li.className = 'path-item';
        li.innerHTML = `<span>${nodeData.action}</span> <span class="path-item-cost">${nodeData.cost} Km</span>`;
        listContainer.appendChild(li);

        // Prender Nodo en Gráfica
        const cityIndex = nodeData.state.currentCity;
        const domNode = mapNodes[cityIndex];
        domNode.classList.add('visited');
        if (i === path.length - 1) domNode.classList.add('active-pulse');

        // Dibujar linea si hay padre
        if (nodeData.parent) {
            const prevCityIndex = nodeData.parent.state.currentCity;
            drawLine(prevCityIndex, cityIndex, svgContainer);
        }

        // Delay para el DOM (Efecto dramático iterativo)
        await sleep(700);
        li.classList.add('show');
    }

    // Finalizar
    const totalCostValue = document.getElementById('totalCostValue');
    totalCostValue.textContent = `${solutionNode.cost} Km`;
    totalCostValue.classList.add('final');

    btn.innerHTML = "Reiniciar Búsqueda";
    btn.disabled = false;
    btn.onclick = () => {
        btn.onclick = null;
        btn.addEventListener('click', startSolvingAnimation);
        btn.innerHTML = "Descubrir Ruta Óptima";
        clearUIForExecution(true);
    };
}

function drawLine(fromIndex, toIndex, svg) {
    const parentContainer = document.getElementById('mapContainer');
    const width = parentContainer.clientWidth;
    const height = parentContainer.clientHeight;
    
    // Coordenadas en porcentajes a pixeles approx.
    const from = cityCoordinates[fromIndex];
    const to = cityCoordinates[toIndex];

    const x1 = (from.x / 100) * width;
    const y1 = (from.y / 100) * height;
    const x2 = (to.x / 100) * width;
    const y2 = (to.y / 100) * height;

    // SVG Line (Se dibujará recta)
    const line = document.createElementNS('http://www.w3.org/2000/svg', 'line');
    line.setAttribute('x1', x1);
    line.setAttribute('y1', y1);
    line.setAttribute('x2', x2);
    line.setAttribute('y2', y2);
    line.setAttribute('class', 'route-line');
    
    svg.appendChild(line);
}

function clearUIForExecution(hardReset = false) {
    document.getElementById('pathList').innerHTML = "";
    document.getElementById('linesContainer').innerHTML = "";
    
    const costValue = document.getElementById('totalCostValue');
    costValue.textContent = "---";
    costValue.classList.remove('final');

    Object.values(mapNodes).forEach(node => {
        node.classList.remove('visited', 'active-pulse');
    });

    if(hardReset) {
        const statusBadge = document.getElementById('statusBadge');
        statusBadge.className = "status-badge standby";
        statusBadge.textContent = "Esperando ejecución...";
    }
}

// Utility
function sleep(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
}
