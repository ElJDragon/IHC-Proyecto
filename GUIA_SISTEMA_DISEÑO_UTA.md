# 🎨 Sistema de Diseño UTA - Guía de Implementación IHC

## 📋 Resumen Ejecutivo

Este sistema de diseño implementa **todos los parámetros de IHC** especificados en los PDFs, usando la paleta de colores institucional de la Universidad Técnica de Ambato (UTA).

---

## 🎯 Parámetros IHC Implementados

### ✅ 1. VARIABLES GLOBALES (Consistencia)
**Regla**: Usa siempre variables CSS, nunca valores hardcoded.
**Implementación**: Todas las variables están en `/css/uta-design-system.css`

```css
/* ❌ MAL - Valor hardcoded */
background-color: #8B1538;

/* ✅ BIEN - Variable CSS */
background-color: var(--color-primary);
```

---

### ✅ 2. AGRUPACIÓN VISUAL (Ley de Región Común)
**Regla**: Datos relacionados dentro de contenedores con borde/sombra.

```html
<!-- Card básico -->
<div class="card">
  <div class="card-header">
    <h2>Título del Grupo</h2>
  </div>
  <div class="card-body">
    <!-- Contenido relacionado -->
  </div>
</div>
```

---

### ✅ 3. ESPACIADO (Ley de Proximidad)
**Regla**: Márgenes consistentes para separar elementos no relacionados.

```html
<!-- Separación entre secciones -->
<div class="mb-xl">Sección 1</div>
<div class="mb-xl">Sección 2</div>

<!-- Gap en contenedores flex -->
<div style="display: flex; gap: var(--spacing-md);">
  <div>Item 1</div>
  <div>Item 2</div>
</div>
```

**Clases de utilidad**:
- `.mt-xs`, `.mt-sm`, `.mt-md`, `.mt-lg`, `.mt-xl` (margin-top)
- `.mb-xs`, `.mb-sm`, `.mb-md`, `.mb-lg`, `.mb-xl` (margin-bottom)
- `.p-xs`, `.p-sm`, `.p-md`, `.p-lg`, `.p-xl` (padding)

---

### ✅ 4. TIPOGRAFÍA (Sans-Serif, 3 niveles)
**Regla**: Una familia (Inter), máximo 3 tamaños principales.

```html
<h1>Título Principal - 24px</h1>
<h2>Subtítulo - 20px</h2>
<p>Texto normal - 16px</p>
<small class="text-small">Texto secundario - 14px</small>
```

---

### ✅ 5. CONTRASTE DE COLOR (WCAG AA/AAA)
**Regla**: Alto contraste entre texto y fondo.

```html
<!-- ✅ BIEN: Negro suave sobre blanco -->
<div style="background: var(--color-surface); color: var(--color-text-primary);">
  Texto legible
</div>

<!-- ❌ MAL: Amarillo sobre blanco -->
<div style="background: white; color: yellow;">
  No legible
</div>
```

**Colores de texto**:
- `--color-text-primary`: Negro suave (#212529) - Contraste AAA
- `--color-text-secondary`: Gris medio (#6C757D) - Contraste AA
- `--color-text-tertiary`: Gris claro (#ADB5BD) - Solo para deshabilitado

---

### ✅ 6. FEEDBACK INMEDIATO (Tiempos de respuesta)
**Regla**: Si una acción tarda >0.1s, muestra indicador visual.

```html
<!-- Botón con spinner -->
<button class="btn btn-primary" disabled="@isLoading">
  @if (isLoading)
  {
    <span class="spinner"></span>
    <span>Guardando...</span>
  }
  else
  {
    <span>Guardar</span>
  }
</button>
```

---

### ✅ 7. ESTADO ACTIVO (Ubicación)
**Regla**: El elemento seleccionado debe ser visualmente claro.

```html
<nav>
  <a href="/dashboard" class="nav-item @(currentPage == "dashboard" ? "active" : "")">
    Dashboard
  </a>
</nav>
```

```css
.nav-item.active {
  background-color: var(--color-primary-light);
  color: var(--color-primary);
  border-left: 3px solid var(--color-primary);
}
```

---

### ✅ 8. AFFORDANCE (Clicabilidad)
**Regla**: Elementos clicables deben tener hover y parecer interactivos.

```html
<button class="btn btn-primary">Soy un botón</button>
```

El CSS automáticamente incluye:
- `:hover` → Cambio de color + elevación
- `:active` → Feedback de click
- `cursor: pointer` → Indica interactividad

---

### ✅ 9. PREVENCIÓN DE ERRORES (Confirmaciones)
**Regla**: Acciones destructivas requieren confirmación.

```html
<!-- Modal de confirmación -->
@if (showDeleteConfirm)
{
  <div class="modal-overlay">
    <div class="card modal-content">
      <h2>¿Eliminar incidente?</h2>
      <p class="text-secondary">Esta acción no se puede deshacer.</p>
      <div style="display: flex; gap: var(--spacing-md);">
        <button class="btn btn-secondary" @onclick="CancelDelete">Cancelar</button>
        <button class="btn btn-danger" @onclick="ConfirmDelete">Eliminar</button>
      </div>
    </div>
  </div>
}
```

---

### ✅ 10. ETIQUETAS VISIBLES (Formularios)
**Regla**: Usa `<label>` visible, nunca solo placeholder.

```html
<!-- ❌ MAL: Solo placeholder -->
<input type="text" placeholder="Nombre de usuario" />

<!-- ✅ BIEN: Label + placeholder -->
<div class="form-group">
  <label class="form-label required">Nombre de usuario</label>
  <input type="text" class="form-input" placeholder="Ej: juan.perez" />
  <small class="form-help-text">Mínimo 5 caracteres</small>
</div>
```

---

### ✅ 11. AYUDA CONTEXTUAL
**Regla**: Muestra requisitos ANTES del error.

```html
<div class="form-group">
  <label class="form-label required">Contraseña</label>
  <input type="password" class="form-input" @bind="password" />
  <!-- Ayuda contextual SIEMPRE visible -->
  <small class="form-help-text">
    Debe contener: 8+ caracteres, 1 mayúscula, 1 número
  </small>
  @if (passwordError)
  {
    <small class="form-error">@passwordError</small>
  }
</div>
```

---

### ✅ 12. CHUNKING (Agrupación 7±2)
**Regla**: Si hay >7 campos, divide en pasos o grupos.

```html
<!-- Wizard de pasos -->
<div class="card">
  <div class="wizard-steps">
    <div class="step @(currentStep == 1 ? "active" : "completed")">
      <span class="step-number">1</span>
      <span>Ubicación</span>
    </div>
    <div class="step @(currentStep == 2 ? "active" : "")">
      <span class="step-number">2</span>
      <span>Tipo de problema</span>
    </div>
    <div class="step @(currentStep == 3 ? "active" : "")">
      <span class="step-number">3</span>
      <span>Detalles</span>
    </div>
  </div>
  
  @if (currentStep == 1)
  {
    <!-- Máximo 5-7 campos aquí -->
  }
</div>
```

---

### ✅ 13. VALORES POR DEFECTO
**Regla**: Usa selects/radios con opciones preseleccionadas.

```html
<div class="form-group">
  <label class="form-label">Prioridad</label>
  <select class="form-select" @bind="priority">
    <option value="media" selected>Media (recomendado)</option>
    <option value="baja">Baja</option>
    <option value="alta">Alta</option>
  </select>
</div>
```

---

### ✅ 14. ICONOS SEMÁNTICOS (Accesibilidad)
**Regla**: `aria-hidden` para decorativos, `aria-label` para funcionales.

```html
<!-- Icono decorativo (acompaña texto) -->
<button class="btn btn-primary">
  <svg aria-hidden="true">...</svg>
  <span>Guardar</span>
</button>

<!-- Icono funcional (sin texto) -->
<button class="btn btn-secondary" aria-label="Cerrar ventana">
  <svg>...</svg>
</button>
```

---

### ✅ 15. NAVEGACIÓN POR TECLADO
**Regla**: Todos los elementos interactivos accesibles con TAB.

```html
<!-- ✅ Elementos nativos son accesibles automáticamente -->
<button class="btn btn-primary">Botón nativo</button>
<a href="/link" class="btn btn-secondary">Link nativo</a>

<!-- ❌ Si usas div como botón, agrega atributos -->
<div role="button" tabindex="0" @onclick="DoSomething" @onkeydown="HandleKeyDown">
  Botón personalizado
</div>
```

---

## 🎨 Paleta de Colores UTA

### Colores Principales
| Variable | Valor | Uso |
|----------|-------|-----|
| `--color-primary` | `#8B1538` | Rojo UTA - Botones principales, enlaces |
| `--color-primary-hover` | `#6B0E28` | Hover de rojo UTA |
| `--color-primary-light` | `#FDF0F3` | Fondos sutiles de rojo |
| `--color-secondary` | `#2C3E50` | Gris azulado institucional |

### Estados (Prioridades)
| Variable | Color | Uso |
|----------|-------|-----|
| `--color-critical` | Rojo | Prioridad crítica |
| `--color-high` | Naranja | Alta prioridad |
| `--color-medium` | Amarillo | Media prioridad |
| `--color-low` | Cyan | Baja prioridad |
| `--color-success` | Verde | Éxito, completado |

---

## 📦 Componentes Listos para Usar

### Botones
```html
<button class="btn btn-primary">Primario - Rojo UTA</button>
<button class="btn btn-secondary">Secundario - Outline</button>
<button class="btn btn-danger">Peligro - Rojo oscuro</button>
<button class="btn btn-success">Éxito - Verde</button>

<!-- Tamaños -->
<button class="btn btn-primary btn-sm">Pequeño</button>
<button class="btn btn-primary">Normal</button>
<button class="btn btn-primary btn-lg">Grande</button>

<!-- Estados -->
<button class="btn btn-primary" disabled>Deshabilitado</button>
```

### Badges (Estados)
```html
<span class="badge badge-critical">Crítica</span>
<span class="badge badge-high">Alta</span>
<span class="badge badge-medium">Media</span>
<span class="badge badge-low">Baja</span>
<span class="badge badge-success">Completado</span>
<span class="badge badge-pending">Pendiente</span>
```

### Cards
```html
<div class="card">
  <div class="card-header">
    <h2>Título</h2>
  </div>
  <div class="card-body">
    Contenido principal
  </div>
  <div class="card-footer">
    <button class="btn btn-primary">Acción</button>
  </div>
</div>
```

### Formularios
```html
<div class="form-group">
  <label class="form-label required">Campo obligatorio</label>
  <input type="text" class="form-input" />
  <small class="form-help-text">Texto de ayuda contextual</small>
</div>

<div class="form-group">
  <label class="form-label">Select</label>
  <select class="form-select">
    <option>Opción 1</option>
  </select>
</div>

<div class="form-group">
  <label class="form-label">Textarea</label>
  <textarea class="form-textarea" rows="4"></textarea>
</div>
```

---

## 🚀 Ejemplo Completo: Formulario IHC-Compliant

```html
<div class="card">
  <div class="card-header">
    <h1>Reportar Incidente</h1>
    <p class="text-secondary">Complete todos los campos obligatorios</p>
  </div>
  
  <div class="card-body">
    <!-- Paso 1: Ubicación (Chunking - máximo 7 campos) -->
    <div class="mb-xl">
      <h2 class="mb-md">
        <span class="step-number">1</span>
        Ubicación del Equipo
      </h2>
      
      <div class="form-group">
        <label class="form-label required">Laboratorio</label>
        <select class="form-select" @bind="laboratorio">
          <option value="">Seleccione un laboratorio</option>
          <option value="lab1">Laboratorio 1</option>
        </select>
        <small class="form-help-text">Seleccione el lab donde está el equipo</small>
      </div>
      
      <div class="form-group">
        <label class="form-label required">Número de Equipo</label>
        <input type="number" class="form-input" @bind="equipoNumero" 
               min="1" max="30" placeholder="Ej: 15" />
        <small class="form-help-text">Número visible en el equipo (1-30)</small>
      </div>
    </div>
    
    <!-- Paso 2: Tipo de Problema (Affordance - cards clicables) -->
    <div class="mb-xl">
      <h2 class="mb-md">
        <span class="step-number">2</span>
        Tipo de Problema
      </h2>
      
      <div style="display: grid; grid-template-columns: 1fr 1fr; gap: var(--spacing-md);">
        <div class="card option-card @(tipoProblema == "hardware" ? "selected" : "")"
             @onclick='() => tipoProblema = "hardware"'
             style="cursor: pointer;">
          <svg aria-hidden="true">...</svg>
          <div>
            <h3>Hardware</h3>
            <p class="text-secondary text-small">Teclado, mouse, pantalla</p>
          </div>
        </div>
        
        <div class="card option-card @(tipoProblema == "software" ? "selected" : "")"
             @onclick='() => tipoProblema = "software"'
             style="cursor: pointer;">
          <svg aria-hidden="true">...</svg>
          <div>
            <h3>Software</h3>
            <p class="text-secondary text-small">Programas, sistema operativo</p>
          </div>
        </div>
      </div>
    </div>
    
    <!-- Paso 3: Descripción -->
    <div class="mb-xl">
      <h2 class="mb-md">
        <span class="step-number">3</span>
        Descripción del Problema
      </h2>
      
      <div class="form-group">
        <label class="form-label required">Descripción detallada</label>
        <textarea class="form-textarea" @bind="descripcion" 
                  rows="4" placeholder="Describa qué sucedió..."></textarea>
        <small class="form-help-text">
          Sea específico: ¿Qué estaba haciendo? ¿Qué mensaje apareció?
        </small>
        @if (descripcionError)
        {
          <small class="form-error">@descripcionError</small>
        }
      </div>
    </div>
  </div>
  
  <div class="card-footer" style="display: flex; justify-content: space-between;">
    <button class="btn btn-secondary" @onclick="Cancelar">
      Cancelar
    </button>
    <button class="btn btn-primary" @onclick="EnviarReporte" disabled="@isLoading">
      @if (isLoading)
      {
        <span class="spinner"></span>
        <span>Enviando...</span>
      }
      else
      {
        <span>Enviar Reporte</span>
      }
    </button>
  </div>
</div>

@code {
  private string laboratorio = "";
  private int equipoNumero;
  private string tipoProblema = "";
  private string descripcion = "";
  private string descripcionError = "";
  private bool isLoading = false;
  
  private async Task EnviarReporte()
  {
    // Validación con ayuda contextual
    if (string.IsNullOrWhiteSpace(descripcion) || descripcion.Length < 20)
    {
      descripcionError = "La descripción debe tener al menos 20 caracteres";
      return;
    }
    
    // Feedback inmediato
    isLoading = true;
    StateHasChanged();
    
    try
    {
      await Task.Delay(1000); // Simula llamada API
      // Enviar datos...
    }
    finally
    {
      isLoading = false;
    }
  }
}
```

---

## ✅ Checklist de Implementación

Al crear un nuevo componente, verifica:

- [ ] **Variables CSS**: Uso de `var(--variable)` en lugar de valores hardcoded
- [ ] **Agrupación Visual**: Datos relacionados dentro de `.card`
- [ ] **Espaciado**: Márgenes consistentes con clases de utilidad
- [ ] **Tipografía**: `<h1>`, `<h2>`, `<p>` con tamaños estándar
- [ ] **Contraste**: Texto oscuro sobre fondo claro (WCAG AA mínimo)
- [ ] **Feedback**: Spinner para acciones >0.1s
- [ ] **Estado Activo**: Clase `.active` en elemento seleccionado
- [ ] **Hover**: Cambio visual en elementos clicables
- [ ] **Confirmación**: Modal para acciones destructivas
- [ ] **Labels**: Siempre visible, no solo placeholder
- [ ] **Ayuda Contextual**: Requisitos visibles ANTES del error
- [ ] **Chunking**: Máximo 7 campos por paso/grupo
- [ ] **Valores por Defecto**: Opciones preseleccionadas inteligentes
- [ ] **Iconos**: `aria-hidden="true"` o `aria-label`
- [ ] **Teclado**: Navegación con TAB funciona correctamente

---

## 📚 Referencias Teóricas

1. **Nielsen Norman Group** - Heurísticas de Usabilidad
2. **WCAG 2.1** - Contraste de Color (Nivel AA/AAA)
3. **Ley de Región Común** - Gestalt (Agrupación Visual)
4. **Ley de Proximidad** - Gestalt (Espaciado)
5. **Miller's Law** - 7±2 elementos en memoria de trabajo
6. **Affordance** - Norman - Señales visuales de interacción
7. **Tiempos de Respuesta** - Card, Moran, Newell (1983)

---

## 🎯 Resultado Esperado

Al aplicar este sistema de diseño:

✅ **Consistencia**: Todos los componentes se ven y funcionan igual  
✅ **Usabilidad**: Reducción de curva de aprendizaje  
✅ **Accesibilidad**: WCAG AA mínimo, navegación por teclado  
✅ **Identidad UTA**: Paleta de colores institucional  
✅ **Mantenibilidad**: Cambios centralizados en variables CSS  
✅ **Cumplimiento IHC**: 100% de parámetros implementados
