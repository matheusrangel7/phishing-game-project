// Configurações
const API_BASE = "http://localhost:5276/api";

// Elementos
const setupScreen = document.getElementById('setupScreen');
const loadingScreen = document.getElementById('loadingScreen');
const gameScreen = document.getElementById('gameScreen');
const resultsScreen = document.getElementById('resultsScreen');
const startBtn = document.getElementById('startBtn');
const phishingBtn = document.getElementById('phishingBtn');
const legitBtn = document.getElementById('legitBtn');
const playAgainBtn = document.getElementById('playAgainBtn');
const globalTimerEl = document.getElementById('globalTimer');
const emailTimerEl = document.getElementById('emailTimer');
const currentEmailEl = document.getElementById('currentEmail');
const totalEmailsEl = document.getElementById('totalEmails');
const emailContainer = document.getElementById('emailContainer');
const scoreSpan = document.getElementById('scoreSpan');
const totalSpan = document.getElementById('totalSpan');
const correctSpan = document.getElementById('correctSpan');
const wrongSpan = document.getElementById('wrongSpan');
const unansweredSpan = document.getElementById('unansweredSpan');
const timeTakenSpan = document.getElementById('timeTakenSpan');
const detailsContainer = document.getElementById('detailsContainer');
const feedbackContainer = document.getElementById('feedbackContainer');
const errorContainer = document.getElementById('errorContainer');

// Variáveis do jogo
let gameSession = null;
let currentEmails = [];
let currentEmailIndex = 0;
let globalTimerInterval = null;
let emailTimerInterval = null;
let globalTimeLeft = 0;
let emailTimeLeft = 0;
let userAnswers = [];

// Iniciar jogo
startBtn.addEventListener('click', async () => {
    const difficulty = document.getElementById('difficulty').value;

    setupScreen.classList.add('hidden');
    loadingScreen.classList.remove('hidden');
    globalTimerEl.classList.add('hidden');

    try {
        const response = await fetch(`${API_BASE}/game/start`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ difficulty })
        });

        if (!response.ok) {
            throw new Error(await response.text());
        }

        gameSession = await response.json();
        currentEmails = gameSession.emails;
        currentEmailIndex = 0;
        userAnswers = [];
        globalTimeLeft = gameSession.totalTimeSeconds;
        emailTimeLeft = gameSession.timePerEmail;

        loadingScreen.classList.add('hidden');
        gameScreen.classList.remove('hidden');
        globalTimerEl.classList.remove('hidden');

        // Atualizar displays
        updateGlobalTimerDisplay();
        emailTimerEl.textContent = emailTimeLeft.toFixed(1);
        currentEmailEl.textContent = currentEmailIndex + 1;
        totalEmailsEl.textContent = currentEmails.length;

        // Iniciar timers
        globalTimerInterval = setInterval(() => {
            globalTimeLeft--;
            updateGlobalTimerDisplay();

            if (globalTimeLeft <= 0) {
                endGame();
            }
        }, 1000);

        startEmailTimer();

        showCurrentEmail();
    } catch (error) {
        handleError(error, setupScreen);
    }
});

// Atualiza a exibição do timer global
function updateGlobalTimerDisplay() {
    globalTimerEl.textContent = globalTimeLeft;
    globalTimerEl.className = 'global-timer';

    if (globalTimeLeft <= 20) {
        globalTimerEl.classList.add('warning');
    }
    if (globalTimeLeft <= 10) {
        globalTimerEl.classList.add('danger');
    }
}

// Responder
phishingBtn.addEventListener('click', () => answer(true));
legitBtn.addEventListener('click', () => answer(false));

function answer(isPhishing) {
    // Feedback visual imediato
    const feedbackEl = document.createElement('div');
    feedbackEl.className = `answer-feedback ${isPhishing ? 'phishing' : 'legit'}`;
    feedbackEl.textContent = isPhishing ? 'Marcado como Phishing' : 'Marcado como Legítimo';
    feedbackContainer.appendChild(feedbackEl);

    // Desabilitar botões para evitar múltiplos cliques
    phishingBtn.disabled = true;
    legitBtn.disabled = true;

    setTimeout(() => {
        feedbackEl.remove();

        userAnswers.push({
            emailId: currentEmails[currentEmailIndex].id,
            userIsPhishing: isPhishing
        });

        currentEmailIndex++;
        currentEmailEl.textContent = currentEmailIndex + 1;

        if (currentEmailIndex < currentEmails.length) {
            emailTimeLeft = gameSession.timePerEmail;
            emailTimerEl.textContent = emailTimeLeft.toFixed(1);
            startEmailTimer();

            // Animação de transição entre e-mails
            emailContainer.classList.add('fade-out');
            setTimeout(() => {
                showCurrentEmail();
                emailContainer.classList.remove('fade-out');

                // Reativar botões para o próximo e-mail
                phishingBtn.disabled = false;
                legitBtn.disabled = false;
            }, 300);
        } else {
            endGame();
        }
    }, 500);
}

function startEmailTimer() {
    clearInterval(emailTimerInterval);

    emailTimerInterval = setInterval(() => {
        emailTimeLeft--;
        emailTimerEl.textContent = emailTimeLeft.toFixed(1);

        // Atualizar classes de aviso
        emailTimerEl.className = 'email-timer';
        if (emailTimeLeft <= 5) {
            emailTimerEl.classList.add('warning');
        }
        if (emailTimeLeft <= 3) {
            emailTimerEl.classList.add('danger');
        }

        if (emailTimeLeft <= 0) {
            clearInterval(emailTimerInterval);
            userAnswers.push({
                emailId: currentEmails[currentEmailIndex].id,
                userIsPhishing: null
            });

            currentEmailIndex++;
            currentEmailEl.textContent = currentEmailIndex + 1;

            if (currentEmailIndex < currentEmails.length) {
                emailTimeLeft = gameSession.timePerEmail;
                emailTimerEl.textContent = emailTimeLeft.toFixed(1);

                // Animação de transição
                emailContainer.classList.add('fade-out');
                setTimeout(() => {
                    startEmailTimer();
                    showCurrentEmail();
                    emailContainer.classList.remove('fade-out');
                }, 300);
            } else {
                endGame();
            }
        }
    }, 1000);
}

function showCurrentEmail() {
    const email = currentEmails[currentEmailIndex];

    // Sanitizar e formatar o corpo do e-mail
    const safeBody = escapeHTML(email.body)
        .replace(/\n/g, '<br>')
        .replace(/https?:\/\/[^\s]+/g, '<span class="fake-link">$&</span>');

    emailContainer.innerHTML = `
        <div class="email-header">
            <p><strong>De:</strong> ${escapeHTML(email.senderName)} &lt;${escapeHTML(email.senderEmail)}&gt;</p>
            <p><strong>Assunto:</strong> ${escapeHTML(email.subject)}</p>
        </div>
        <div class="email-body">${safeBody}</div>
    `;
}

// Função para escapar HTML (prevenção XSS)
function escapeHTML(str) {
    return str
        .replace(/&/g, '&amp;')
        .replace(/</g, '&lt;')
        .replace(/>/g, '&gt;')
        .replace(/"/g, '&quot;')
        .replace(/'/g, '&#39;');
}

async function endGame() {
    clearInterval(globalTimerInterval);
    clearInterval(emailTimerInterval);

    // Registrar e-mails não respondidos restantes
    for (let i = currentEmailIndex; i < currentEmails.length; i++) {
        userAnswers.push({
            emailId: currentEmails[i].id,
            userIsPhishing: null
        });
    }

    try {
        // Criar array válido para envio
        const payload = userAnswers.map(a => ({
            emailId: a.emailId,
            userIsPhishing: a.userIsPhishing
        }));

        const response = await fetch(`${API_BASE}/game/submit?sessionId=${gameSession.sessionId}`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(payload)
        });

        if (!response.ok) {
            throw new Error(await response.text());
        }

        const result = await response.json();
        showResults(result);
    } catch (error) {
        handleError(error, gameScreen);
    }
}

function showResults(result) {
    gameScreen.classList.add('hidden');
    resultsScreen.classList.remove('hidden');
    globalTimerEl.classList.add('hidden');

    scoreSpan.textContent = result.score;
    totalSpan.textContent = result.totalPossible;
    correctSpan.textContent = result.correctAnswers;

    // Calcular erros (respostas erradas)
    const wrongAnswers = result.details.filter(d => d.wasAnswered && !d.wasCorrect).length;
    wrongSpan.textContent = wrongAnswers;

    unansweredSpan.textContent = result.unanswered;
    timeTakenSpan.textContent = result.timeTaken;

    detailsContainer.innerHTML = '';

    result.details.forEach(detail => {
        const email = currentEmails.find(e => e.id === detail.emailId);
        if (!email) return;

        // Encontrar a resposta do usuário para este e-mail
        const userAnswerObj = userAnswers.find(a => a.emailId === detail.emailId);
        const userAnswerText = userAnswerObj ?
            (userAnswerObj.userIsPhishing === null ? "Não respondido" :
                (userAnswerObj.userIsPhishing ? "Phishing" : "Legítimo")) : "Não registrado";

        const resultItem = document.createElement('div');

        if (!detail.wasAnswered) {
            resultItem.className = 'result-item unanswered';
            resultItem.innerHTML = `
                <p><strong>E-mail:</strong> ${escapeHTML(email.subject)}</p>
                <p>⏱ Não respondido</p>
                <div class="explanation">
                    <p><strong>Análise:</strong> ${escapeHTML(email.explanation)}</p>
                </div>
            `;
        } else {
            const wasCorrect = detail.wasCorrect;

            resultItem.className = wasCorrect ? 'result-item correct' : 'result-item incorrect';
            resultItem.innerHTML = `
                <p><strong>E-mail:</strong> ${escapeHTML(email.subject)}</p>
                <p><strong>Sua resposta:</strong> ${userAnswerText}</p>
                <p>${wasCorrect ? '✅ Correto' : '❌ Errado'}</p>
                <div class="explanation">
                    <p><strong>Análise:</strong> ${escapeHTML(email.explanation)}</p>
                </div>
            `;
        }

        detailsContainer.appendChild(resultItem);
    });
}

// Tratamento de erros
function handleError(error, fallbackScreen) {
    console.error(error);
    const errorMessage = error.message || 'Erro desconhecido';

    // Exibir alerta
    const alertEl = document.createElement('div');
    alertEl.className = 'error-alert';
    alertEl.innerHTML = `
        <p>⚠️ Ocorreu um erro:</p>
        <p>${escapeHTML(errorMessage)}</p>
        <button>OK</button>
    `;
    errorContainer.appendChild(alertEl);

    // Configurar botão para remover o alerta
    alertEl.querySelector('button').addEventListener('click', () => {
        alertEl.remove();
    });

    // Voltar para a tela apropriada
    document.querySelectorAll('.card').forEach(el => el.classList.add('hidden'));
    fallbackScreen.classList.remove('hidden');
}

// Reiniciar jogo
playAgainBtn.addEventListener('click', showSetupScreen);

function showSetupScreen() {
    resultsScreen.classList.add('hidden');
    setupScreen.classList.remove('hidden');
    globalTimerEl.classList.add('hidden');
}