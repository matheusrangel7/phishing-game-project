window.onload = function () {
    const feedback = document.getElementById("feedbackAnimation");
    const resultado = feedback?.getAttribute("data-resultado")?.trim() ?? "";

    feedback.className = "feedback hidden";
    feedback.textContent = "";

    if (!resultado) return;

    if (resultado.startsWith("✅")) {
        feedback.textContent = "+1";
        feedback.classList.remove("hidden", "negativo");
        feedback.classList.add("show", "positivo");
    } else if (resultado.startsWith("❌")) {
        feedback.textContent = "✖";
        feedback.classList.remove("hidden", "positivo");
        feedback.classList.add("show", "negativo");
    }

    setTimeout(() => {
        feedback.className = "feedback hidden";
        feedback.textContent = "";
    }, 1200);
};
