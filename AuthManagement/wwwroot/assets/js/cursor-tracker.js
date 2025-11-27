(function () {
    if (typeof window.registerCursorTracker === "function") {
        // Already defined, skip redefining
        return;
    }

    function registerCursorTracker(options) {
        if (!options || !options.input) return;

        const input = options.input;
        const delimiter = options.delimiter || '';
        let lastValue = input.value;
        let lastCursor = input.selectionStart || 0;

        input.addEventListener('keydown', function () {
            lastCursor = input.selectionStart || 0;
        });

        input.addEventListener('input', function () {
            const newValue = input.value;
            const diff = newValue.length - lastValue.length;
            const leftOfCursor = newValue.slice(0, input.selectionStart);
            const delimiterCount = (leftOfCursor.match(new RegExp(escapeRegExp(delimiter), 'g')) || []).length;

            let newCursor = lastCursor + diff;
            if (delimiter && diff > 0 && newCursor < newValue.length) {
                newCursor += delimiterCount;
            }

            lastValue = newValue;
            setCursorPosition(input, newCursor);
        });

        function setCursorPosition(element, position) {
            if (element.setSelectionRange) {
                element.focus();
                element.setSelectionRange(position, position);
            }
        }

        function escapeRegExp(string) {
            return string.replace(/[.*+?^${}()|[\]\\]/g, '\\$&');
        }
    }

    // ✅ Safely attach to global object
    window.registerCursorTracker = registerCursorTracker;
})();
