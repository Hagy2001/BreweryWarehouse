// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

document.addEventListener("DOMContentLoaded", () => {
    document.querySelectorAll('.bw-datepicker').forEach(initDatePicker);

    // Close popups when clicking outside
    document.addEventListener('click', (e) => {
        if (!e.target.closest('.bw-datepicker')) {
            document.querySelectorAll('.bw-datepicker-calendar').forEach(c => {
                c.classList.remove('is-open');
                c.setAttribute('hidden', '');
            });
        }
    });
});

$(function () {
    if ($.validator) {
        $.validator.setDefaults({
            onfocusout: function (element) {
                $(element).valid();
            },
            onkeyup: false
        });
    }
});

$(function () {
    var $input = $('#bw-global-search-input');
    var $results = $('#bw-global-search-results');

    if (!$input.length) return;

    var searchTimer;

    $input.on('input', function () {
        clearTimeout(searchTimer);
        var q = $input.val().trim();
        if (q.length < 2) { closeGlobalSearch(); return; }
        searchTimer = setTimeout(function () {
            $.getJSON('/search/global', { q: q })
                .done(renderGlobalSearchResults)
                .fail(closeGlobalSearch);
        }, 300);
    });

    function renderGlobalSearchResults(items) {
        $results.empty();
        if (!items || !items.length) { $results.hide(); return; }

        var currentCategory = null;
        items.forEach(function (item) {
            if (item.category !== currentCategory) {
                currentCategory = item.category;
                $results.append(
                    $('<li class="bw-global-search__group-label"></li>').text(item.category)
                );
            }
            var $item = $('<li class="bw-global-search__item" role="option"></li>');
            $item.append($('<span class="bw-global-search__item-label"></span>').text(item.label));
            if (item.subtitle) {
                $item.append($('<span class="bw-global-search__item-sub"></span>').text(item.subtitle));
            }
            $item.on('click', function () { window.location.href = item.url; });
            $results.append($item);
        });
        $results.show();
    }

    function closeGlobalSearch() {
        $results.hide().empty();
    }

    $(document).on('click', function (e) {
        if (!$(e.target).closest('.bw-global-search').length) closeGlobalSearch();
    });

    $input.on('keydown', function (e) {
        if (e.key === 'Escape') { closeGlobalSearch(); $input.blur(); }
    });
});

function initDatePicker(container) {
    const displayInput = container.querySelector('.bw-datepicker__display');
    const hiddenInput = container.querySelector('.bw-datepicker__hidden');
    const toggleBtn = container.querySelector('.bw-datepicker__toggle');
    const calendar = container.querySelector('.bw-datepicker-calendar');
    const culture = container.getAttribute('data-culture');
    
    // UI elements inside calendar
    const grid = calendar.querySelector('.bw-datepicker__grid');
    const prevMonthBtn = calendar.querySelector('.bw-datepicker__prev');
    const nextMonthBtn = calendar.querySelector('.bw-datepicker__next');
    const monthYearBtn = calendar.querySelector('.bw-datepicker__month-year-btn');
    const yearPicker = calendar.querySelector('.bw-datepicker__year-picker');
    const yearGrid = calendar.querySelector('.bw-datepicker__year-grid');
    const decadeLabel = calendar.querySelector('.bw-datepicker__decade-label');
    const prevDecadeBtn = calendar.querySelector('.bw-datepicker__prev-decade');
    const nextDecadeBtn = calendar.querySelector('.bw-datepicker__next-decade');
    let yearPickerOpen = false;
    let decadeStart = Math.floor(new Date().getFullYear() / 10) * 10;

    let currentDate = new Date(); // Active viewing month
    let selectedDate = parseIsoDate(hiddenInput.value);
    let focusDate = selectedDate ? new Date(selectedDate.getTime()) : new Date();

    if (selectedDate) {
        currentDate = new Date(selectedDate.getTime());
    }

    function toggleCalendar() {
        const isOpen = calendar.classList.contains('is-open');
        if (isOpen) {
            closeCalendar();
        } else {
            openCalendar();
        }
    }

    function openCalendar() {
        // Close others
        document.querySelectorAll('.bw-datepicker-calendar').forEach(c => {
            if (c !== calendar) {
                c.classList.remove('is-open');
                c.setAttribute('hidden', '');
            }
        });
        
        calendar.classList.add('is-open');
        calendar.removeAttribute('hidden');
        if (selectedDate) focusDate = new Date(selectedDate.getTime());
        // Reset year picker state
        yearPickerOpen = false;
        yearPicker.setAttribute('hidden', '');
        grid.removeAttribute('hidden');

        // Flip above if not enough space below
        const inputRect = container.getBoundingClientRect();
        const spaceBelow = window.innerHeight - inputRect.bottom;
        if (spaceBelow < 300) {
            calendar.classList.add('is-above');
        } else {
            calendar.classList.remove('is-above');
        }

        renderCalendar();
    }

    function closeCalendar() {
        calendar.classList.remove('is-open');
        calendar.setAttribute('hidden', '');
    }

    function parseIsoDate(str) {
        if (!str) return null;
        const pts = str.split('T')[0].split('-');
        if (pts.length !== 3) return null;
        return new Date(pts[0], parseInt(pts[1]) - 1, pts[2]);
    }

    function formatIsoDate(d) {
        if (!d) return '';
        const y = d.getFullYear();
        const m = String(d.getMonth() + 1).padStart(2, '0');
        const day = String(d.getDate()).padStart(2, '0');
        return `${y}-${m}-${day}`;
    }

    function parseDisplayDate(str, cul) {
        if (!str) return null;
        let day, month, year;
        if (cul === 'hr') {
            // dd.mm.yyyy
            const m = str.match(/^(\d{1,2})\.(\d{1,2})\.(\d{4})$/);
            if (!m) return null;
            day = parseInt(m[1]); month = parseInt(m[2]); year = parseInt(m[3]);
        } else {
            // mm/dd/yyyy
            const m = str.match(/^(\d{1,2})\/(\d{1,2})\/(\d{4})$/);
            if (!m) return null;
            month = parseInt(m[1]); day = parseInt(m[2]); year = parseInt(m[3]);
        }
        if (month < 1 || month > 12 || day < 1 || day > 31 || year < 1900 || year > 2100) return null;
        const d = new Date(year, month - 1, day);
        // verify no overflow (e.g. Feb 31)
        if (d.getMonth() !== month - 1 || d.getDate() !== day) return null;
        return d;
    }

    function formatDisplayDate(d, cul) {
        if (!d) return '';
        const day = String(d.getDate()).padStart(2, '0');
        const m = String(d.getMonth() + 1).padStart(2, '0');
        const y = d.getFullYear();
        
        return (cul === 'hr') ? `${day}.${m}.${y}` : `${m}/${day}/${y}`;
    }

    function renderCalendar() {
        grid.innerHTML = '';
        
        // Month Year header
        const monthNames = ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'];
        monthYearBtn.textContent = `${monthNames[currentDate.getMonth()]} ${currentDate.getFullYear()}`;

        // Days of week header
        const daysOfWeek = (culture === 'hr') ? ['Mo', 'Tu', 'We', 'Th', 'Fr', 'Sa', 'Su'] : ['Su', 'Mo', 'Tu', 'We', 'Th', 'Fr', 'Sa'];
        daysOfWeek.forEach(d => {
            const cell = document.createElement('div');
            cell.className = 'bw-datepicker__cell bw-datepicker__cell--header';
            cell.textContent = d;
            grid.appendChild(cell);
        });

        // Dates
        const y = currentDate.getFullYear();
        const m = currentDate.getMonth();
        const firstDay = new Date(y, m, 1);
        const lastDay = new Date(y, m + 1, 0);
        
        let startGrid = firstDay.getDay(); 
        if (culture === 'hr') {
            startGrid = startGrid === 0 ? 6 : startGrid - 1; // Monday first
        }

        // Blank days
        for (let i = 0; i < startGrid; i++) {
            const cell = document.createElement('div');
            cell.className = 'bw-datepicker__cell bw-datepicker__cell--empty';
            grid.appendChild(cell);
        }

        const today = new Date();
        
        for (let day = 1; day <= lastDay.getDate(); day++) {
            const dStr = formatIsoDate(new Date(y, m, day));
            const btn = document.createElement('button');
            btn.type = 'button';
            btn.className = 'bw-datepicker__cell bw-datepicker__cell--day';
            btn.textContent = day;
            btn.setAttribute('data-date', dStr);
            btn.tabIndex = -1;

            if (selectedDate && day === selectedDate.getDate() && m === selectedDate.getMonth() && y === selectedDate.getFullYear()) {
                btn.classList.add('is-selected');
            }
            if (focusDate && day === focusDate.getDate() && m === focusDate.getMonth() && y === focusDate.getFullYear()) {
                btn.classList.add('is-focused');
            }
            if (day === today.getDate() && m === today.getMonth() && y === today.getFullYear()) {
                btn.classList.add('is-today');
            }

            btn.addEventListener('click', () => {
                selectDate(new Date(y, m, day));
                closeCalendar();
            });

            grid.appendChild(btn);
        }
    }

    function renderYearPicker() {
        yearGrid.innerHTML = '';
        decadeLabel.textContent = `${decadeStart}–${decadeStart + 11}`;
        const today = new Date();
        for (let y = decadeStart; y <= decadeStart + 11; y++) {
            const btn = document.createElement('button');
            btn.type = 'button';
            btn.className = 'bw-datepicker__year-cell';
            btn.textContent = y;
            if (y === today.getFullYear()) btn.classList.add('is-current');
            if (selectedDate && y === selectedDate.getFullYear()) btn.classList.add('is-selected');
            btn.addEventListener('click', (e) => {
                e.stopPropagation();
                currentDate = new Date(y, currentDate.getMonth(), 1);
                toggleYearPicker(); // closes year picker, re-renders calendar
            });
            yearGrid.appendChild(btn);
        }
    }

    function toggleYearPicker() {
        if (yearPickerOpen) {
            yearPicker.setAttribute('hidden', '');
            grid.removeAttribute('hidden');
            yearPickerOpen = false;
            monthYearBtn.textContent = `${['January','February','March','April','May','June','July','August','September','October','November','December'][currentDate.getMonth()]} ${currentDate.getFullYear()}`;
            renderCalendar();
        } else {
            yearPicker.removeAttribute('hidden');
            grid.setAttribute('hidden', '');
            decadeStart = Math.floor(currentDate.getFullYear() / 10) * 10;
            yearPickerOpen = true;
            renderYearPicker();
        }
    }

    function selectDate(d) {
        selectedDate = new Date(d.getTime());
        focusDate = new Date(d.getTime());
        const iso = formatIsoDate(d);
        hiddenInput.value = iso;
        displayInput.value = formatDisplayDate(d, culture);
        // Dispatch change event for validation/other listeners
        hiddenInput.dispatchEvent(new Event('change', { bubbles: true }));
        displayInput.dispatchEvent(new Event('change', { bubbles: true }));
    }

    // Set initial display
    if (hiddenInput.value) {
        const pd = parseIsoDate(hiddenInput.value);
        if (pd) displayInput.value = formatDisplayDate(pd, culture);
    }

    // Event Listeners
    displayInput.addEventListener('click', openCalendar);
    displayInput.addEventListener('focus', openCalendar);

    displayInput.addEventListener('input', () => {
        const raw = displayInput.value.trim();
        const parsed = parseDisplayDate(raw, culture);
        if (parsed) {
            selectedDate = parsed;
            currentDate = new Date(parsed.getFullYear(), parsed.getMonth(), 1);
            focusDate = new Date(parsed.getTime());
            hiddenInput.value = formatIsoDate(parsed);
            if (calendar.classList.contains('is-open')) {
                renderCalendar();
            }
            hiddenInput.dispatchEvent(new Event('change', { bubbles: true }));
        } else if (raw === '') {
            selectedDate = null;
            hiddenInput.value = '';
            if (calendar.classList.contains('is-open')) renderCalendar();
            hiddenInput.dispatchEvent(new Event('change', { bubbles: true }));
        }
    });

    if (toggleBtn) {
        toggleBtn.addEventListener('click', (e) => { e.stopPropagation(); toggleCalendar(); });
    }

    prevMonthBtn.addEventListener('click', (e) => {
        e.stopPropagation();
        currentDate.setMonth(currentDate.getMonth() - 1);
        renderCalendar();
    });

    nextMonthBtn.addEventListener('click', (e) => {
        e.stopPropagation();
        currentDate.setMonth(currentDate.getMonth() + 1);
        renderCalendar();
    });

    monthYearBtn.addEventListener('click', (e) => {
        e.stopPropagation();
        toggleYearPicker();
    });

    prevDecadeBtn.addEventListener('click', (e) => {
        e.stopPropagation();
        decadeStart -= 12;
        renderYearPicker();
    });

    nextDecadeBtn.addEventListener('click', (e) => {
        e.stopPropagation();
        decadeStart += 12;
        renderYearPicker();
    });

    displayInput.addEventListener('keydown', (e) => {
        if (!calendar.classList.contains('is-open') && ['ArrowDown', 'ArrowUp', 'ArrowLeft', 'ArrowRight', 'Enter'].includes(e.key)) {
            openCalendar();
            e.preventDefault();
            return;
        }

        if (!calendar.classList.contains('is-open')) return;

        let fd = focusDate || new Date(currentDate.getFullYear(), currentDate.getMonth(), 1);

        if (e.key === 'ArrowDown') {
            fd.setDate(fd.getDate() + 7);
            e.preventDefault();
        } else if (e.key === 'ArrowUp') {
            fd.setDate(fd.getDate() - 7);
            e.preventDefault();
        } else if (e.key === 'ArrowLeft') {
            fd.setDate(fd.getDate() - 1);
            e.preventDefault();
        } else if (e.key === 'ArrowRight') {
            fd.setDate(fd.getDate() + 1);
            e.preventDefault();
        } else if (e.key === 'Enter') {
            selectDate(fd);
            closeCalendar();
            e.preventDefault();
            return;
        } else if (e.key === 'Escape') {
            closeCalendar();
            e.preventDefault();
            return;
        }

        // If month changed via cross-boundary keyboard moves
        if (fd.getMonth() !== currentDate.getMonth() || fd.getFullYear() !== currentDate.getFullYear()) {
            currentDate = new Date(fd.getFullYear(), fd.getMonth(), 1);
        }
        
        focusDate = new Date(fd.getTime());
        renderCalendar();
    });

    calendar.addEventListener('keydown', (e) => {
        if (e.key === 'Escape') {
            closeCalendar();
            displayInput.focus();
        }
    });

    // Prevent clsoing when clicking inside calendar
    calendar.addEventListener('click', (e) => {
        e.stopPropagation();
    });
}
